﻿using EventZone.Helpers;
using EventZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventZone.Controllers
{
    public class LookAroundController : Controller
    {

        public ActionResult Index() {
            return View();
        }
        public ActionResult LookAround(double longitude = 0, double latitude = 0, double distance = 20.0, int categoryID=-1) {
         
                List<Location> listPlace = new List<Location>();
                Location cur = new Location();
                cur.LocationID = -1;
                cur.LocationName = "Current Location";
                cur.Longitude = longitude;
                cur.Latitude = latitude;
                //if (Request.Form["search"] != null)
                //{
                //    cur.Longitude = Double.Parse(Request.Form["latitude"]);
                //    cur.Latitude = Double.Parse(Request.Form["longitude"]);
                //}
                listPlace = LocationHelpers.Instance.GetAllLocation();
                listPlace.RemoveAll(item => (LocationHelpers.Instance.CalculateDistance(cur, item) - distance) > 0);// tat ca cac dia diem o gan
                List<EventPlace> listEventPlace = new List<EventPlace>();
                List<EventPlace> nearlyEventPlace = new List<EventPlace>();
                listEventPlace = LocationHelpers.Instance.GetAllEventPlace();
                foreach (var item in listPlace)
                {
                    var abc = listEventPlace.FindAll(a => a.LocationID == item.LocationID);
                    if (abc != null)
                        foreach (var x in abc)
                        {
                            nearlyEventPlace.Add(x);
                        }
                }
                List<Location> nearlyLocation = new List<Location>();
                List<Event> nearlyEvent = new List<Event>();
                nearlyEventPlace = nearlyEventPlace.Distinct().ToList();
                foreach (var item in nearlyEventPlace)
                {
                    var evt = EventDatabaseHelper.Instance.GetEventByID(item.EventID);
                    if (categoryID == -1)
                    {
                        var place = LocationHelpers.Instance.GetLocationById(item.LocationID);
                        nearlyLocation.Add(place);
                        nearlyEvent.Add(evt);
                    }
                    else if (evt.CategoryID == categoryID) {
                        var place = LocationHelpers.Instance.GetLocationById(item.LocationID);
                        nearlyLocation.Add(place);
                        nearlyEvent.Add(evt);
                    }
                }
      
                nearlyEvent = nearlyEvent.Distinct().ToList();
                nearlyLocation = LocationHelpers.Instance.RemovePlaceByDistance(nearlyLocation, 1); 
                ViewData["currentLocation"] = cur;
                ViewData["nearlyEvent"] = EventDatabaseHelper.Instance.GetThumbEventListByListEvent(nearlyEvent);
                ViewData["nearlyLocation"] = nearlyLocation.Distinct().ToList();
                return PartialView("_LookAround");           
        }
        public ActionResult showEventInLocation(long id, int categoryID=-1)
        {

            if (Request.IsAjaxRequest())
            {
                Location  currLocation= LocationHelpers.Instance.GetLocationById(id);
                
                  List<Location> listPlace = LocationHelpers.Instance.GetAllLocation();
                  listPlace.RemoveAll(item => (LocationHelpers.Instance.CalculateDistance(currLocation, item) - 0.5) >= 0);
                  List<EventPlace> listEventPlace= new List<EventPlace>();
                  
                  foreach (var item in listPlace) {
                      try
                      {
                          var result = (from a in LocationHelpers.Instance.GetAllEventPlace() where a.LocationID == item.LocationID select a).ToList();
                          if (result != null) {
                              listEventPlace.AddRange(result);
                          } 
                      }
                      catch { }
                  }
                List<Event> listEventSamePlace = new List<Event>();
                foreach (var item in listEventPlace)
                {
                    var evt = EventDatabaseHelper.Instance.GetEventByID(item.EventID);
                    if (categoryID == -1) {
                        listEventSamePlace.Add(evt);
                    }
                    else if (evt.CategoryID == categoryID) {
                        listEventSamePlace.Add(evt);
                    }
                }
               listEventSamePlace = listEventSamePlace.Distinct().ToList();
                List<ViewThumbEventModel> listThumb = EventDatabaseHelper.Instance.GetThumbEventListByListEvent(listEventSamePlace);
                return PartialView("_ViewEventInPlace", listThumb);
            }
            return null;
            //return RedirectToAction("ShowMap", "Event", new {id = 1});
        }
       
    }
}
