$(document).ready(function(){
	
	/* ---------- Login Box Styles ---------- */
	if($(".login-box")) {
		
		$("#username").focus(function() {
			$(this).parent(".input-prepend").addClass("input-prepend-focus");
		});

		$("#username").focusout(function() {
			$(this).parent(".input-prepend").removeClass("input-prepend-focus");
		});

		$("#password").focus(function() {
			$(this).parent(".input-prepend").addClass("input-prepend-focus");
		});

		$("#password").focusout(function() {
			$(this).parent(".input-prepend").removeClass("input-prepend-focus");
		});
		
	}
					
	/* ---------- Add class .active to current link  ---------- */
	$('ul.main-menu li a').each(function(){
		if($($(this))[0].href==String(window.location))
			$(this).parent().addClass('active');
	});
			
	/* ---------- Activate Functions ---------- */
	template_functions();
	widthFunctions();
});

/* ---------- Template Functions ---------- */
function template_functions() {

    /* ---------- Disable moving to top ---------- */
    $('a[href="#"][data-top!=true]').click(function(e) {
        e.preventDefault();
    });

    /* ---------- Uniform ---------- */
    
}	


/* ---------- Page width functions ---------- */
$(window).bind("resize", widthFunctions);

function widthFunctions( e ) {
    var winHeight = $(window).height();
    var winWidth = $(window).width();

	if (winHeight) {
		$("#content").css("min-height",winHeight);
	}
    
	if (winWidth < 980 && winWidth > 767) {
		if($(".main-menu-span").hasClass("span2")) {
			$(".main-menu-span").removeClass("span2");
			$(".main-menu-span").addClass("span1");
		}
		
		if($("#content").hasClass("span10")) {
			$("#content").removeClass("span10");
			$("#content").addClass("span11");
		}
		
		$("a").each(function(){
			if($(this).hasClass("quick-button-small span1")) {
				$(this).removeClass("quick-button-small span1");
				$(this).addClass("quick-button span2 changed");
			}
		});
		
		$(".circleStatsItem").each(function() {
			var getOnTablet = $(this).parent().attr('onTablet');
			var getOnDesktop = $(this).parent().attr('onDesktop');
			if (getOnTablet) {
				$(this).parent().removeClass(getOnDesktop);
				$(this).parent().addClass(getOnTablet);
			}
		});
		
		$(".box").each(function(){
			var getOnTablet = $(this).attr('onTablet');
			var getOnDesktop = $(this).attr('onDesktop');
			if (getOnTablet) {
				$(this).removeClass(getOnDesktop);
				$(this).addClass(getOnTablet);
			}
		});
							
	}
	else {
		if($(".main-menu-span").hasClass("span1")) {
			$(".main-menu-span").removeClass("span1");
			$(".main-menu-span").addClass("span2");
		}
		
		if($("#content").hasClass("span11")) {
			$("#content").removeClass("span11");
			$("#content").addClass("span10");
		}
		
		$("a").each(function(){
			if($(this).hasClass("quick-button span2 changed")) {
				$(this).removeClass("quick-button span2 changed");
				$(this).addClass("quick-button-small span1");
			}
		});
		
		$(".circleStatsItem").each(function() {
			var getOnTablet = $(this).parent().attr('onTablet');
			var getOnDesktop = $(this).parent().attr('onDesktop');
			if (getOnTablet) {
				$(this).parent().removeClass(getOnTablet);
				$(this).parent().addClass(getOnDesktop);
			}
		});
		
		$(".box").each(function(){
			var getOnTablet = $(this).attr('onTablet');
			var getOnDesktop = $(this).attr('onDesktop');
			if (getOnTablet) {
				$(this).removeClass(getOnTablet);
				$(this).addClass(getOnDesktop);
			}
		});
	}
}