$(document).ready(function(){
	/*** Xử lý ngay khi load trang ***/

    $('.d_txt_1').transition({ scale: 0, delay: 0 });
    $('.d_txt_2').transition({ scale: 0, delay: 0 });
    $('.d_mc_text_2').transition({ scale: 0, delay: 0 });
    $('.d_mc_category').transition({ scale: 0, delay: 0 });

    $('.d_md_body').transition({ marginTop: '80px' });
    $('.d_txt_1').transition({ scale: 1, delay: 1000 }, 'ease'); 
    $('.d_txt_2').transition({ scale: 1, delay: 1200 }, 'ease');
    $('.d_mc_text_2').transition({ scale: 1, delay: 1500 }, 'ease');
    $('.d_mc_category').transition({ scale: 1, delay: 2000 }, 'ease');

});
