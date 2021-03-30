jQuery(document).ready(function ($) {

  //Show/Hide scroll-top on Scroll
  // hide #back-top first
  $("#scroll-top").hide();
  // fade in #back-top
  $(function () {
    $(window).scroll(function () {
      if ($(this).scrollTop() > 100) {
        $('#scroll-top').fadeIn();
      } else {
        $('#scroll-top').fadeOut();
      }
    });
    // scroll body to 0px on click
    $('#scroll-top').click(function () {
      $('body,html').animate({
        scrollTop: 0
      }, 1000);
    });
  });
  $('.nav-toggle').on('click', function (e) {
    $(this).toggleClass('open');
    $('body').toggleClass('menuin');
  });
  $('.nav-overlay').on('click', function (e) {
    $('.nav-toggle').trigger('click');
  })
  $('#main-nav').on('click', '.sub-toggle', function (e) {
    e.preventDefault();
    var li = $(this).parent('li');
    var otherLi = li.siblings();
    var sub = li.children('.side-sub');
    otherLi.children('.side-sub').slideUp();
    sub.stop().slideToggle(function () {
      sub.is(':hidden') ? li.removeClass('sub-in') : li.addClass('sub-in');
    });
  });


  $(window).scroll(function () {
    if ($(window).scrollTop() > $(".page-heading").outerHeight()) {
      $("#header").addClass("sticky");
    } else {
      $("#header").removeClass("sticky");
    }
  }); 

  $('#call-filterbox').on('click', function (e) {
    e.preventDefault();
    $.magnificPopup.open({
      items: {
        src: '#filterbox', // can be a HTML string, jQuery object, or CSS selector
      },
      mainClass: 'filter-over',
      prependTo: '.filter-page-heading',
      callbacks: {
        open: function () {
          var $popC = this.content;

        },
        close: function () {
          var $popC = this.content;
        }
      }
    });
  });

  $('#header').on('click', '.js-submenu-toggle', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var li = $(this).parent();
    $(this).siblings('.mn-submenu').stop().slideToggle();
  })

  $(document).on('click', '.js-open-toggle', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var parent = $(this).parent();
    $(this).toggleClass('active');
    $(this).siblings('.toggle-listmenu').stop().slideToggle();
  })

  $(document).on('click', '.js-par-open-toggle', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var parent = $(this).parents('.toggle-parent');
    console.log(parent);
    $(this).toggleClass('active');
    parent.find('.toggle-listmenu').stop().slideToggle();
  })

  var iniTab = $('.pchild-button li.active .swap-btn').attr('src-tabswap');
  $('.tab-swap-desc' + iniTab).show().siblings().hide();
  $(document).on('click', '.pchild-button .swap-btn', function (e) {
     e.preventDefault();
     e.stopPropagation();
    var srcTab = $(this).attr('src-tabswap');
    $(this).parent().addClass('active').siblings().removeClass('active');
    $('.tab-swap-desc' + srcTab).fadeIn().siblings().hide();
  })

  $('.gift-progess .js-open-popup').on('click', function (e) {
    $('#giftpopup-slider').addClass('fadeIn')
    $('#giftpopup-slider')[0].slick.refresh();
  })

  $(document).on('click', '.js-open-popup', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var src_modal = $(this).attr('src-popup');
    $.magnificPopup.open({
      items: {
        src: src_modal
      },
      type: 'inline',
      fixedContentPos: true,
      closeOnBgClick: false,
      //showCloseBtn: false,
    });
  });

  
  // place left progess text fill
  if ($('.progess-bar .current-fill').length) {
    var $currText = $('.progess-bar .current-fill');
    var $progess = $('.progess-bar .progess-fill');
    if ($progess.outerWidth() - $currText.outerWidth() < 0) {
      $currText.addClass('leftplace');
    }
  }
  
});