
var wiki={
"loadcss":function(v,c){
			var element = document.createElement("link");
			element.href = v;
			element.rel= "stylesheet";
			element.type = "text/css";
			if(c)
			element.charset=c;
			(document.head||document.getElementsByTagName("head")[0]).appendChild(element);
	},
"loadjs":function(v,c){
		var element = document.createElement("script");
		element.src = v;
		element.type = "text/javascript";
		if(c)
		element.charset=c;
		document.body.appendChild(element); 
	},
"setCookie":function(name, value, options){
	if (typeof value != 'undefined') {
		options = options || {};
		if (value === null) {
			value = '';
			options.expires = -1;
		}
		var expires = '';
		if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
			var date;
			if (typeof options.expires == 'number') {
				date = new Date();
				date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
			} else {
				date = options.expires;
			}
			expires = '; expires=' + date.toUTCString();
		}
		var path = options.path ? '; path=' + (options.path) : '';
		var domain = options.domain ? '; domain=' + (options.domain) : '';
		var secure = options.secure ? '; secure': '';
		document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
	} else {
		var cookieValue = null;
		if (document.cookie && document.cookie != '') {
			var cookies = document.cookie.split(';');
			for (var i = 0; i < cookies.length; i++) {
				var cookie = jQuery.trim(cookies[i]);
				if (cookie.substring(0, name.length + 1) == (name + '=')) {
					cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
					break;
				}
			}
		}
		return cookieValue;
	}
 },
"onscroll":function(){
	if (wiki.contentHeight<(wiki.docElement.scrollTop||window.pageYOffset)){
		$(".bottomnav").css({
		"position":"absolute",
		"bottom":"auto",
		"top":$(".mainwrapper").height()+/*35-30*/65
		})
	}else{
		if (!-[1,]&&!window.XMLHttpRequest){ 
		//ie6
			$(".bottomnav").css({
			"bottom":"auto",
			"top":(document.documentElement.scrollTop+document.documentElement.clientHeight-30)	
			});
		
		}else{
		//非ie6	
		$(".bottomnav").css({
		"position":"fixed",
		"bottom":"0",
		"top":"auto"	
		})
		}
	}
},
"bindNavEvent": function (pathname) {
	__inited__ = true;
	$("#mw-panel").find(".portal").css("margin-top","-1px");
	$("#mw-panel").find("h5").click(function () {
		var t = $(this),b = t.next(".body"),p = t.parent();
		if (b.is(":visible")) {
			b.slideUp("fast");
			p.removeClass("active");
		}else{
			b.find("li").find("em").addClass("none");
			b.slideDown("fast",function(){
				b.find("li").find("em").removeClass("none");
			});
			p.addClass("active");
		}
		$(this).parent().siblings(".portal").removeClass("active").find(".body").slideUp("fast");
	});
    
},
"initDoc":function(){
	$("#bodyContent").find("a").each(function(){
		var url = $(this).attr("href");
		if (/^(http|https):\/\//.test(url)){
			$(this).attr("target","_blank");
		}
		
	});
	$("#bodyContent").find(".paratable").find("a").each(function(){
		var url = $(this).attr("href");
		if (/^[^#]/.test(url)){
		$(this).attr("target","_blank");
		}
	});
	wiki.onscroll();
	$(".bottomnav").fadeIn("slow");
}
};
var __inited__ = false;
$(function(){
wiki.docElement=document.documentElement||document.body;
__inited__ || wiki.bindNavEvent(location.pathname);
setTimeout(function(){
	wiki.contentHeight=65/*35+30*/+$(".mainwrapper").height()-wiki.docElement.clientHeight;
	window.onscroll=wiki.onscroll;
	wiki.initDoc();
},1000);
$("body").mouseover(function(event){
	var target = event.target,
		subnavlist = $(".subnav"),
		currentSubNav = (function(){
			for(var i=0,k=subnavlist.length;i<k;i++){
				if ($.contains(subnavlist[i],target) || target === subnavlist[i]){
					return subnavlist[i];
				}
			}
			return false;
		})();
	if (currentSubNav){
		$(currentSubNav).addClass("subnav_hover");
	}else{
		subnavlist.removeClass("subnav_hover");
	}
});
});
