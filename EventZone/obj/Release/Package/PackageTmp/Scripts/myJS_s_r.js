$(document).ready(function(){
/*********** xử lý khi Load trang ***/

/*********** Hover chuột để mở Tab 3 items ***/
    $('.d_3_tabs tr td').mouseenter(function () {    //Show 3 Tabs, đẩy Khung Result sang
        $('.d_3_tabs').transition({ x: '-180px' });
        $('.body-content').transition({ marginLeft: '1.5%' });
        $('.d_white_bg').transition({ marginLeft: '1.5%' });
    });
    //$('.d_3_tabs').z_mouseleave(function () {    //Hide 3 Tabs, đẩy Khung Result lại
    //    $('.d_3_tabs').transition({ x: '0px' });
    //    $('.body-content').transition({ marginLeft: '10%' });
    //    $('.d_white_bg').transition({ marginLeft: '10%' });
    //}); 
});