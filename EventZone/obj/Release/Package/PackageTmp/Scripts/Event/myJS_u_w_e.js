$(document).ready(function(){

/******************************************** SCRIPT xử lý ngay khi Load trang ***/
	$('.d_cmt_after').css("display", "none");	//Hide Comment
	$('.d_reopen_map').css("display", "none"); //Hide nút Re-open Map
	$('.d_reasons_container').css("display", "none"); //Hide Comment

	$('.d_btn_close').css("display", "none"); //Hide Close Button

    /******************************************** Animation Show more video ***/
	$('#i_video_tab .d_btn_more').click(function () {    //Show Other Videos
	    $('#i_video_tab iframe').animate({  // Co video chính lên
	        height: '253px',
	        width: '452px'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_video_tab .d_vid_info').animate({ //Kéo info của Video vào
	        width: '88%'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_video_tab .d_btn_close').show("fast"); // Hiện nút Đóng
	    $('#i_video_tab .d_btn_more').hide("fast");  // Mất nút Mở
	});
    /***************************************************** Append video vào video chính***/

    /******************************************** Animation Close other video ***/
	$('#i_video_tab .d_btn_close').click(function () {   //Đóng Other Videos
	    $('#i_video_tab iframe').animate({  // Phóng to Video chính
	        height: '353px',
	        width: '630px'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_video_tab .d_vid_info').animate({ //Đẩy Info của Video theo
	        width: '100%'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_video_tab .d_btn_close').hide("fast"); //Mất nút Đóng
	    $('#i_video_tab .d_btn_more').show("fast");  //Hiện nút Mở
	});
    /******************************************** Animation Show more LIVE video ***/
	$('#i_live_vid_tab .d_btn_more').click(function () {    //Show Other Videos
	    $('#i_live_vid_tab iframe').animate({  // Co video chính lên
	        height: '253px',
	        width: '452px'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_live_vid_tab .d_vid_info').animate({ //Kéo info của Video vào
	        width: '88%'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_live_vid_tab .d_btn_close').show("fast"); // Hiện nút Đóng
	    $('#i_live_vid_tab .d_btn_more').hide("fast");  // Mất nút Mở
	});
    /******************************************** Animation Close other LIVE video ***/
	$('#i_live_vid_tab .d_btn_close').click(function () {   //Đóng Other Videos
	    $('#i_live_vid_tab iframe').animate({  // Phóng to Video chính
	        height: '353px',
	        width: '630px'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_live_vid_tab .d_vid_info').animate({ //Đẩy Info của Video theo
	        width: '100%'
	    }, {
	        duration: 300,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    $('#i_live_vid_tab .d_btn_close').hide("fast"); //Mất nút Đóng
	    $('#i_live_vid_tab .d_btn_more').show("fast");  //Hiện nút Mở
	});

/******************************************** Animation Mở Comment, Hide Map ***/
	$('.d_txt_write_cmt').click(function(){
		$('.d_right_bottom').animate({
		    height: '460px'
		}, {
		    duration: 600,
		    queue: false,   //queue: false giúp các Animation chạy đồng thời
		    complete: function() { /* Animation complete */ }
		});
		var d = $('#comment-content');
		d.scrollTop(d.prop("scrollHeight"));
		$('.d_right_top').slideToggle( "slow" ); //Hide Map đi
		$('.d_cmt_before').slideToggle( "slow" ); //Hide Sample comment
		$('.d_cmt_after').slideToggle( "slow" ); //Display Comment
		$('.d_reopen_map').slideToggle("slow"); //Display nút Re-open Map
		$('.d_rp_before').slideToggle("slow"); //Hide Sample report
		$('.d_reasons_container').slideToggle("slow"); //Display Comment
	});

/******************************************** Animation Hide Comment, Mở Map ***/
	$('.d_reopen_map').click(function(){
		$('.d_right_bottom').animate({
		    height: '150px'
		}, {
		    duration: 600,
		    queue: false,   //queue: false giúp các Animation chạy đồng thời
		    complete: function() { /* Animation complete */ }
		});
		$('.d_right_top').slideToggle( "slow" ); //Mở lại Map
		$('.d_cmt_before').slideToggle( "slow" ); //Mở lại Sample comment
		$('.d_cmt_after').slideToggle( "slow" ); //Hide Comment
		$('.d_reopen_map').slideToggle("slow"); //Hide nút Re-open Map
		$('.d_rp_before').slideToggle("slow"); //Hide Sample report
		$('.d_reasons_container').slideToggle("slow"); //Display Comment
	});

/******************************************** Animation Mở Comment, Hide Map ***/
	$('.d_rp_fade').click(function () {
	    $('.d_right_bottom').animate({
	        height: '460px'
	    }, {
	        duration: 600,
	        queue: false,   //queue: false giúp các Animation chạy đồng thời
	        complete: function () { /* Animation complete */ }
	    });
	    var d = $('#comment-content');
	    d.scrollTop(d.prop("scrollHeight"));
	    $('.d_right_top').slideToggle("slow"); //Hide Map đi
	    $('.d_cmt_before').slideToggle("slow"); //Hide Sample comment
	    $('.d_cmt_after').slideToggle("slow"); //Display Comment
	    $('.d_reopen_map').slideToggle("slow"); //Display nút Re-open Map
	    $('.d_rp_before').slideToggle("slow"); //Hide Sample report
	    $('.d_reasons_container').slideToggle("slow"); //Display Comment
	});

});
    function LockEvent(eventID) {
        $.ajax({
            url: "/Admin/LockEvent",
            type: "POST",
            dataType: "json",
            data: { eventID: eventID, reason: "I dont want your event in my website xD" },
            success: function (data) {
                if (data.state == 0) {
                    errorShow(data.error, data.message)
                } else {
                    $("#lockUnlockDiv-" + eventID).empty();
                    $("#lockUnlockDiv-" + eventID).prepend('<button class="btn btn-danger btn-xs" onclick="UnlockEvent(' + eventID + ')" id="lockUnlock-' + eventID + '">Unlock</button>')
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            }
        })
    }
//unlockEvent
function UnlockEvent(eventID) {
    $.ajax({
        url: "/Admin/UnlockEvent",
        type: "POST",
        dataType: "json",
        data: { eventID: eventID },
        success: function (data) {
            if (data.state == 0) {
                errorShow(data.error, data.message)
            } else {
                $("#lockUnlockDiv-" + eventID).empty();
                $("#lockUnlockDiv-" + eventID).prepend('<button class="btn btn-success btn-xs" onclick="LockEvent(' + eventID + ')" id="lockUnlock-' + eventID + '" >Lock</button>')
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
        }
    })
}
