$(document).ready(function(){

$("a").mouseover(function(){ $(this).css("color","blue");});
$("a").mouseout(function(){ $(this).css("color","gray");});
$("a").click(function(){$(this).css("color", "red");});

});