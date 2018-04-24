﻿using EventZone.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventZone.Models;
using System.Net;
using System.Data.Entity;
using System.Web.Helpers;
using System.IO;
using Amazon.S3;

namespace EventZone.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// View User index của user có userID= userID,
        /// nếu userID = -1 thì view trang quản lý user của người đăng nhập, 
        /// nếu user chưa đăng nhập và userID=-1 thì trở về trang Home
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult Index(long userID = -1)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (userID == -1)
            {
                if (user == null)
                {
                    if (Request.Cookies["userName"] != null && Request.Cookies["password"] != null)
                    {
                        string userName = Request.Cookies["userName"].Value;
                        string password = Request.Cookies["password"].Value;
                        if (UserDatabaseHelper.Instance.ValidateUser(userName, password))
                        {
                            user = UserDatabaseHelper.Instance.GetUserByUserName(userName);
                            if (UserDatabaseHelper.Instance.isLookedUser(user.UserName))
                            {
                                TempData["errorTitle"] = "Locked User";
                                TempData["errorMessage"] = "Your account is locked! Please contact with our support";
                            }
                            else
                            {
                                UserHelpers.SetCurrentUser(Session, user);
                            }

                        }
                    }
                }
            }
            else
            {
                user = UserDatabaseHelper.Instance.GetUserByID(userID);
                if (user == null)
                {
                    TempData["errorTitle"] = "Not Avaiable";
                    TempData["errorMessage"] = "Ops.. This user does not exists in the system!";
                    return RedirectToAction("Index", "Home");
                }
                else if (user.AccountStatus == EventZoneConstants.Lock)
                {
                    TempData["errorTitle"] = "Locked User";
                    TempData["errorMessage"] = "Ops.. This user has been locked!";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }
        /// <summary>
        /// view report
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult ViewReport(long eventID, int type = -1, long userID = -1)
        {
            List<Report> listReport = new List<Report>();
            listReport = EventDatabaseHelper.Instance.GetEventReport(eventID, type, userID);
            TempData["typeView"] = type;
            return PartialView("ViewReport", listReport);
        }
        /// <summary>
        /// view appeal
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult ViewAppeal(long eventID, int type = -1)
        {
            List<Appeal> listAppeal = new List<Appeal>();
            listAppeal = EventDatabaseHelper.Instance.GetEventAppeal(eventID, type);
            TempData["typeView"] = type;
            return PartialView("_ViewAppeal", listAppeal);
        }
        public JsonResult Like(long eventId)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            Boolean success = false;
            int state = EventZoneConstants.NotRate;

            if (user != null)
            {
                LikeDislike findlike = UserDatabaseHelper.Instance.FindLike(user.UserID, eventId);
                if (findlike != null)
                {
                    state = findlike.Type;
                }
                success = UserDatabaseHelper.Instance.InsertLike(user.UserID, eventId);
            }

            return Json(new
            {
                success = success,
                state = state
            });
        }

        public JsonResult DisLike(long eventId)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            Boolean success = false;
            int state = EventZoneConstants.NotRate;
            if (user != null)
            {
                LikeDislike findlike = UserDatabaseHelper.Instance.FindLike(user.UserID, eventId);
                if (findlike != null)
                {
                    state = findlike.Type;
                }
                success = UserDatabaseHelper.Instance.InsertDislike(user.UserID, eventId);
            }
            return Json(new
            {
                success = success,
                state = state
            });
        }
        public JsonResult FollowEvent(long eventId)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            Boolean success = false;
            int followState = 0;// trang thai khong follow
            if (user != null)
            {
                success = UserDatabaseHelper.Instance.FollowEvent(user.UserID, eventId);
                if (UserDatabaseHelper.Instance.IsFollowingEvent(user.UserID, eventId))
                {
                    //trang thai tu unfollow sang follow
                    followState = 1;
                }
            }
            return Json(new
            {
                success = success,
                state = followState
            }
           );
        }
        public JsonResult FollowUser(long userID)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (user != null)
            {
                if (UserDatabaseHelper.Instance.FollowPeople(user.UserID, userID))
                {
                    NotificationDataHelpers.Instance.SendNotiNewFollower(userID, user.UserID);
                    return Json(new
                    {
                        state = 1,
                        userID = userID
                    });
                };
            }
            return Json(new
            {
                state = 0,
                error = "Error",
                message = "Something wrong! Please reload page and try again!"
            });
        }
        public JsonResult UnFollowUser(long userID)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (user != null)
            {
                if (UserDatabaseHelper.Instance.UnFollowPeople(user.UserID, userID))
                {
                    return Json(new
                    {
                        state = 1,
                        userID = userID
                    });
                };
            }
            return Json(new
            {
                state = 0,
                error = "Error",
                message = "Something wrong! Please reload page and try again!"
            });
        }


        public JsonResult FollowCategory(long categoryId)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            Boolean success = false;
            int followState = 0;
            if (user != null)
            {
                success = UserDatabaseHelper.Instance.FollowCategory(user.UserID, categoryId);
                if (UserDatabaseHelper.Instance.IsFollowingCategory(user.UserID, categoryId))
                {
                    followState = 1;
                }
            }
            return Json(new
            {
                success = success,
                state = followState
            }

           );
        }

        public ActionResult UploadAvatar(HttpPostedFileBase file)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (user == null)
            {
                TempData["errorTitle"] = "Require Signin";
                TempData["errorMessage"] = "Ops.. It look like you are current is not signed in system! Please sign in first!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Image photo = new Image();
                try
                {
                    if (file != null)
                    {
                        string[] whiteListedExt = { ".jpg", ".gif", ".png", ".tiff",".jpeg" };
                        Stream stream = file.InputStream;
                        string extension = Path.GetExtension(file.FileName);
                        if (whiteListedExt.Contains(extension))
                        {
                            string pic = Guid.NewGuid() + user.UserID.ToString() + extension;
                            using (AmazonS3Client s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2))
                                EventZoneUtility.FileUploadToS3("eventzone", pic, stream, true, s3Client);
                            Image image = new Image();
                            image.ImageLink = "https://s3-us-west-2.amazonaws.com/eventzone/" + pic;
                            image.UserID = user.UserID;
                            image.UploadDate = DateTime.Today;
                            if (UserDatabaseHelper.Instance.UpdateAvatar(user, image))
                            {
                                TempData["errorTitle"] = null;
                                TempData["errorMessage"] = null;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["errorTitle"] = "Database Error";
                                TempData["errorMessage"] = "Ops... Some error is ocurred while we save to database! Please try again later!";
                                return RedirectToAction("Index");
                            }

                        }
                        else
                        {
                            TempData["errorTitle"] = "Database Error";
                            TempData["errorMessage"] = "Ops... Some error is ocurred while we save to database! Please try again later!";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["errorTitle"] = "Not select file";
                        TempData["errorMessage"] = "It look like you forgot select an image! Are you getting old?";
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData["errorTitle"] = "Unknow Error";
                    TempData["errorMessage"] = "Oops..Something wrong is happened! Please try again later...";
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult ChangePassword()
        {
            return PartialView();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordPost(ChangePasswordView chgpwd)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    state = 0,
                    message = "Invalid model"
                });
            User user = UserHelpers.GetCurrentUser(Session);
            if (user == null)
            {

                return Json(new
                {
                    state = 0,
                    message = "require signin"
                });
            }
            else
            {
                if (!UserDatabaseHelper.Instance.ValidateUser(user.UserName, chgpwd.OldPassword))
                {

                    return Json(new
                    {
                        state = 0,
                        message = "wrong old passsword"
                    });

                }
                else if (UserDatabaseHelper.Instance.ChangePassword(user, chgpwd.NewPassword))
                {
                    return Json(new
                    {
                        state = 1,
                        message = ""
                    });
                }
                else
                {
                    return Json(new
                    {
                        state = 0,
                        message = "something wrong!"
                    });
                }
            }

        }
        public ActionResult UpdateInfo()
        {
            User userSession = UserHelpers.GetCurrentUser(Session);

            if (userSession == null)
            {
                TempData["errorTitle"] = "Require Signin";
                TempData["errorMessage"] = "Ops.. It look like you are current is not signed in system! Please sign in first!";
                return RedirectToAction("Index", "Home");
            }
            EditUserModel editUserModel = new EditUserModel();
            editUserModel.UserID = userSession.UserID;
            editUserModel.UserDOB = userSession.UserDOB;
            editUserModel.UserFirstName = userSession.UserFirstName;
            editUserModel.UserLastName = userSession.UserLastName;
            editUserModel.Phone = userSession.Phone;
            editUserModel.Place = userSession.Place;
            editUserModel.IDCard = userSession.IDCard;
            return PartialView(editUserModel);

        }
        public ActionResult UpdateInfoPost(EditUserModel editUserModel)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    User user = UserHelpers.GetCurrentUser(Session);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "You are signed out!Please signin to do this!");
                        return RedirectToAction("EditProfile");
                    }
                    user.UserFirstName = editUserModel.UserFirstName;
                    user.UserLastName = editUserModel.UserLastName;
                    user.UserDOB = editUserModel.UserDOB;
                    user.IDCard = editUserModel.IDCard;
                    user.Gender = editUserModel.Gender;
                    user.Phone = editUserModel.Phone;
                    user.Place = editUserModel.Place;

                    if (UserDatabaseHelper.Instance.UpdateUser(user))
                    {
                        UserHelpers.SetCurrentUser(Session, user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something went wrong! Please try again!");
                        return RedirectToAction("EditProfile");
                    }
                }
                ModelState.AddModelError("", "Something went wrong! Please try again!");
                return RedirectToAction("EditProfile");
            }
            catch (Exception ex)
            {
                TempData["ManageProfileTask"] = "EditProfile";
                ModelState.AddModelError("", "Something went wrong! Please try again!");
                return RedirectToAction("EditProfile");
            }
        }
        public ActionResult ManageFollow(long userID = -1)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (userID == -1)
            {
                if (user == null)
                {
                    TempData["errorTitle"] = "Require Signin";
                    TempData["errorMessage"] = "Ops.. It look like you are current is not signed in system! Please sign in first!";
                    return PartialView("_ManageFollow");
                }
                else userID = user.UserID;
            }
            List<ViewThumbEventModel> listviewThumb = EventDatabaseHelper.Instance.GetThumbEventListByListEvent(UserDatabaseHelper.Instance.GetFollowingEvent(userID));
            List<User> listFollowingUser = UserDatabaseHelper.Instance.GetListFollowingOfUser(userID);
            List<User> listFollower = UserDatabaseHelper.Instance.GetListFollowerOfUser(userID);
            TempData["ListEventThumb"] = listviewThumb;
            TempData["ListFollowing"] = listFollowingUser;
            TempData["ListFollower"] = listFollower;
            TempData["ViewingUser"] = UserDatabaseHelper.Instance.GetUserByID(userID);
            return PartialView("_ManageFollow");
        }
        public ActionResult ReportEvent(int typeReport = -1, string reportContent = "", long eventId = -1)
        {
            if (eventId == -1 || typeReport == -1)
            {
                return Json(new
                {
                    state = 0,
                    error = "Error",
                    message = "Somthing wrong..."
                });
            }

            User user = UserHelpers.GetCurrentUser(Session);
            if (user == null)
            {
                return Json(new
                {
                    state = 0,
                    error = "Require Signin",
                    message = "Ops.. It look like you are current is not signed in system! Please sign in first!",
                });
            }
            else if (user.UserID == EventDatabaseHelper.Instance.GetAuthorEvent(eventId).UserID)
            {
                return Json(new
                {
                    state = 0,
                    error = "Error",
                    message = "You cant report your event!",
                });
            }
            else
            {

                Report newReport = UserDatabaseHelper.Instance.ReportEvent(user.UserID, eventId, typeReport, reportContent);
                if (newReport != null)
                {
                    NotificationDataHelpers.Instance.SendNotiNewReport(EventDatabaseHelper.Instance.GetAuthorEvent(eventId).UserID, user.UserID, eventId);

                    return Json(new
                    {
                        state = 1,
                        newReportType = newReport.ReportType,
                        newReportDate = newReport.ReportDate.ToString()
                    });
                }
                return Json(new
                {
                    state = 0,
                    error = "Error",
                    message = "Somthing wrong..."
                });

            }

        }
        public ActionResult Event(long userID = -1)
        {

            User user = UserHelpers.GetCurrentUser(Session);

            if (userID == -1)
            {
                if (user == null)
                {
                    if (Request.Cookies["userName"] != null && Request.Cookies["password"] != null)
                    {
                        string userName = Request.Cookies["userName"].Value;
                        string password = Request.Cookies["password"].Value;
                        if (UserDatabaseHelper.Instance.ValidateUser(userName, password))
                        {
                            user = UserDatabaseHelper.Instance.GetUserByUserName(userName);
                            if (UserDatabaseHelper.Instance.isLookedUser(user.UserName))
                            {
                                TempData["errorTitle"] = "Locked User";
                                TempData["errorMessage"] = "Your account is locked! Please contact with our support";
                            }
                            else
                            {
                                UserHelpers.SetCurrentUser(Session, user);
                            }

                        }
                    }
                }
                if (user == null)
                {
                    TempData["errorTitle"] = "Require Signin";
                    TempData["errorMessage"] = "Ops.. It look like you are current is not signed in system! Please sign in first!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                if (user != null && user.UserID == userID)
                {
                    return RedirectToAction("Event", "User");
                }
                user = UserDatabaseHelper.Instance.GetUserByID(userID);
                if (user == null)
                {
                    TempData["errorTitle"] = "Not Avaiable";
                    TempData["errorMessage"] = "Ops.. This user does not exists in the system!";
                    return RedirectToAction("Index", "Home");
                }
                else if (user.AccountStatus == EventZoneConstants.Lock)
                {
                    TempData["errorTitle"] = "Locked User";
                    TempData["errorMessage"] = "Ops.. This user has been locked!";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }
        public ActionResult Appeal(long eventID, string content)
        {
            User user = UserHelpers.GetCurrentUser(Session);
            if (user == null)
            {
                return Json(new
                {
                    state = 0,
                    error = "Require Signin",
                    message = "Ops.. It look like you are current is not signed in system! Please sign in first!",
                });
            }
            else if (user.UserID != EventDatabaseHelper.Instance.GetAuthorEvent(eventID).UserID)
            {
                return Json(new
                {
                    state = 0,
                    error = "Error",
                    message = "You cant not appeal an event which is created by another users",
                });
            }
            else
            {
                Appeal newAppeal = new Appeal
                {
                    AppealStatus = EventZoneConstants.Pending,
                    EventID = eventID,
                    SendDate = DateTime.Now,
                    AppealContent = content,
                };
                if (EventDatabaseHelper.Instance.AddNewAppeal(newAppeal))
                {
                    return Json(new
                    {
                        state = 1,
                        eventID = eventID
                    });
                }
                else
                {
                    return Json(new
                    {
                        state = 0,
                        error = "error",
                        message = "Something wrong! please try again later!   "
                    });
                }
            }

        }
        public ActionResult PagingEventManage(long userID, int page, bool isOwner, string keyword = "")
        {
            List<Event> listEvent = UserDatabaseHelper.Instance.GetUserEvent(userID, -1, isOwner);
            if (!string.IsNullOrEmpty(keyword))
            {
                listEvent = listEvent.FindAll(model => CommonDataHelpers.Instance.MatchKeyWord(keyword, model));

            }
            int startIndex = (page - 1) * 10;
            int endIndex = (listEvent.Count) < (page * 10) ? (listEvent.Count - 1) : (page * 10) - 1;
            if (startIndex > endIndex)
            {
                return null;
            }
            if (listEvent == null)
            {
                TempData["LoadMore"] = false;
                return PartialView("_UserThumbnail", null);
            }
            if (listEvent.Count > (page * 10))
            {
                TempData["LoadMore"] = true;
            }
            else
            {
                TempData["LoadMore"] = false;
            }
            List<Event> listView = new List<Event>();
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                listView.Add(listEvent[i]);
            }
            return PartialView("_EventPage", listView);
        }
        public ActionResult PagingReportManage(int page, string keyword = "")
        {
            User user = UserHelpers.GetCurrentUser(Session);
            List<Event> listEventHasReports = UserDatabaseHelper.Instance.GetAllEventHasReports(user.UserID);
            if (!string.IsNullOrEmpty(keyword)) {
                listEventHasReports = listEventHasReports.FindAll(model => CommonDataHelpers.Instance.MatchKeyWord(keyword, model));
            }
            int startIndex = (page - 1) * 10;
            int endIndex = (listEventHasReports.Count) < (page * 10) ? (listEventHasReports.Count - 1) : (page * 10) - 1;
            if (startIndex > endIndex)
            {
                return null;
            }
            if (listEventHasReports == null)
            {
                TempData["LoadMore"] = false;
                return PartialView("_ViewReport", null);
            }
            if (listEventHasReports.Count > (page * 10))
            {
                TempData["LoadMore"] = true;
            }
            else
            {
                TempData["LoadMore"] = false;
            }
            List<Event> listView = new List<Event>();
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                listView.Add(listEventHasReports[i]);
            }

            return PartialView("_ViewReport", listView);
        }
        public ActionResult NotifyDisplay()
        {
            return PartialView("_NotificationDisplay");
        }
        public ActionResult ReadNotification(int notificationChangeID)
        {
            NotificationChange item = NotificationDataHelpers.Instance.GetNotificationChangeByID(notificationChangeID);
            User user = UserHelpers.GetCurrentUser(Session);
            int notiType = NotificationDataHelpers.Instance.GetNotificationObjectByID(item.NotificationObjectID).Type;
            if (notiType == EventZoneConstants.FollowingUserAddNewEvent)
            {
                NotificationDataHelpers.Instance.ReadNotification(item);
            }
            else if (notiType == EventZoneConstants.CommentNotification)
            {
                NotificationDataHelpers.Instance.ReadNotifiComment(item, user.UserID);
            }
            else if (notiType == EventZoneConstants.NewFollower)
            {
                NotificationDataHelpers.Instance.ReadNotifiNewFollow(item, user.UserID);
            }
            else if (notiType == EventZoneConstants.RequestUploadImage)
            {

                NotificationDataHelpers.Instance.ReadNotifiRequestUploadImage(item);

            }
            else if (notiType == EventZoneConstants.EventHasBeenLocked)
            {
                NotificationDataHelpers.Instance.ReadNotification(item);
            }
            else if (notiType == EventZoneConstants.EventHasBeenUnLocked)
            {
                NotificationDataHelpers.Instance.ReadNotification(item);
                //<li>
                //    <a onlick="ReadNoty(@item.ID)" href="@Url.Action("Details", "Event", new {id = item.EventID})">Good new! Your event @EventDatabaseHelper.Instance.GetEventByID(item.EventID).EventName has been unlocked</a>
                //</li>
                //<li role="separator" class="divider"></li>
            }
            else if (notiType == EventZoneConstants.ReportNotification)
            {

                NotificationDataHelpers.Instance.ReadNotifiReport(item, user.UserID);
            }
            return Json(new
            {

            });
        }
        public ActionResult ChangeAboutMe(long userID, string newContent)
        {
            if (UserDatabaseHelper.Instance.ChangeChannelDescription(userID, newContent))
            {
                return Json(new
                {
                    state = 1
                });
            }
            else {
                return Json(new
                {
                    state = 0
                });
            }
        }

    }

}

