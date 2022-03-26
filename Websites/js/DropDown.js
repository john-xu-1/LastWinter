// JavaScript Document
var bodyCommentSpacing = 190;
var unitCellUnityCanvas = "<iframe src=\"UnitCell/index.html\" width=\"540px\" height=\"460px\" ></iframe> \
							<p>Input Miller indices to display a plane in the unit cell.</p>";
var unitCellLoaded = false;

	function resizeFunction(){
		var wrapperWidth = document.getElementById("bodyCommentWrapper").offsetWidth;
		var width = (wrapperWidth - bodyCommentSpacing) + 'px';
		//document.getElementById("bodyWrapper").style.width = width;	
		
		
		var bodyElements = document.getElementsByClassName("bodyWrapper");
		for (i=0; i < bodyElements.length; i++){
			bodyElements[i].style.width = width;	
		}
		
		var height = document.getElementById("bodyCommentWrapper").offsetHeight;
		var str = height + 'px';
		//document.getElementById("console").style.top = str;c v
		document.getElementById("bodyCommentWrapper2").innerHTML = str;
	}



function loadMenu(){
	
	//bottom level
	let LastWinter = ['link', 'LastWinter', 'Last Winter'];
	let LastWinterThesis = ['link', 'thesis/LastWinter/', 'ASP Thesis'];
	let Gerrymandering = ['link', 'https://apps.apple.com/us/app/gerrymandering-game/id1612116902', 'Gerrymandering Game'];
	let VexIQ = ['link', 'https://apps.apple.com/us/app/vex-iq-robotics-simulation/id1546022908', 'Vex IQ Simulation'];
	let WhisperOfThePast = ['link', 'https://apps.apple.com/us/app/whisper-of-the-past/id1541775012', 'Whisper of the Past'];
	let LottieEscape = ['link', 'https://apps.apple.com/us/app/lottie-escape/id1573066692', 'Lottie: Escape'];
	let SafeDrive = ['link', 'https://apps.apple.com/us/app/safe-drive-test/id1547907856', 'Safe Drive'];
	let SafetyLifetime = ['link', 'https://apps.apple.com/us/app/safety-lifetime/id1507479875', 'Safety Lifetime'];
	let MasqueradeOfMiasma = ['link', 'https://store.steampowered.com/app/1293950/Masquerade_of_Miasma/', 'Masquerade of Miasma'];
	let IttyGrav = ['link', 'ittygrav/', 'IttyGrav'];
	let SixteenLittleGods = ['link', 'sixteenlittlegods', 'Sixteen Little Gods'];
	
	//level 2
	let CodingMinds = ['flyout', 'Coding Minds', LastWinter, Gerrymandering, VexIQ, WhisperOfThePast, LottieEscape, SafeDrive, SafetyLifetime];
	let WinchellGames = ['flyout', 'John Morris', MasqueradeOfMiasma, Gerrymandering, LastWinter, IttyGrav, SixteenLittleGods];
	let WinchellPortfolio = ['link','winchellportfoliolist.html', 'John Morris' ];
	let TamblinThenVarblin = ['link', 'tamblinthenvarblin', 'Tamblin then Varblin'];
	let LastWinterDev = ['flyout', 'Last Winter', LastWinter, LastWinterThesis];
	
	//Top level
	let Home = ['link','index.html', 'Home'];
	let Games = ['flydown', 'Games',CodingMinds, WinchellGames];
	let Development = ['flydown', 'Development', LastWinterDev];
	let About = ['flydown', 'About', WinchellPortfolio];
	let Other = ['flydown', 'Other', TamblinThenVarblin];
	
	let level1 = [Home, Games, Development, About, Other];
	
	createMenu(level1);
}
function createMenu(level1){
		var menuStr = '<ul class=\"level1\" id=\"menuTop\">';
//		menuStr += createMenuLink('index.html', 'Home');
//			menuStr += createMenuFly('Games');
//				menuStr += createMenuDropDown(2);
//					menuStr += createMenuFly('CC');
//						menuStr += createMenuFlyout(1);
//							menuStr += createMenuLink('cc/golfwithyourself', 'Golf with Yourself');
//						menuStr += '</ul>';
//					menuStr += '</li>';
//				menuStr += '</ul>';
//			menuStr += '</li>';
	for(let I = 0; I < level1.length; I++){
		if(level1[I][0] == 'link') {
			menuStr += createMenuLink(level1[I][1], level1[I][2]);
		}else if(level1[I][0] == 'flydown'){
			menuStr += createMenuFly(level1[I][1]);
			menuStr += createMenuDropDown(I + 1);
			for(let A = 0; A < level1[I].length - 2; A++){
				if(level1[I][A+2][0] == 'link'){
					menuStr += createMenuLink(level1[I][A+2][1], level1[I][A+2][2]);
				}else if(level1[I][A+2][0] == 'flyout'){
					menuStr += createMenuFly(level1[I][A+2][1]);
					menuStr += createMenuFlyout(A + 1);
					for(let i = 0; i < level1[I][A+2].length - 2; i++){
						menuStr += createMenuLink(level1[I][A+2][i+2][1], level1[I][A+2][i+2][2]);
					}
					menuStr += '</ul>';
					menuStr += '</li>';
					
				}
				
			}
			menuStr += '</ul>';
			menuStr += '</li>';
		}
	}
	menuStr += '</ul>';
	document.getElementById("menu").innerHTML= menuStr;
}
function createMenuLink(url,label){
	const menuStr = '<li><a href=\"' + url + '\">' + label + '</a></li>';
	return menuStr; // createMenuFly(flyLabel) + '</li>'
}
function createMenuFly(label){
	return '<li><a class=\"fly\" href=\"#url\" tabindex=\"1\">' + label + '</a>'; //+ createMenuDropDown(index) + '</li>'
}
function createMenuDropDown(order){
	return '<ul class=\"dropdown d' + order + '\">';// + '</ul>'
}
function createMenuFlyout(order){
	return '<ul class=\"flyout f' + order + '\">';// + '</ul>'
}
	
	function loadMenuOld(){
		var menuStr = "\
<ul class=\"level1\" id=\"menuTop\"> \
    <li><a href=\"index.html\">Home</a></li>\
\
    <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">Games</a>\
      <ul class=\"dropdown d2\">\
        <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">CC</a>\
          <ul class=\"flyout f1\">\
            <li><a href=\"cc\\scaredflappy\">Scared Flappy</a></li>\
            <li><a href=\"cc\\angryanimals\">Angry Animals</a></li>\
            <li><a href=\"cc\\golfwithyourself\">Golf with Yourself</a></li>\
          </ul>\
        </li>\
        <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">Touchdevelop</a>\
          <ul class=\"flyout f2\">\
            <li><a href=\"https://www.touchdevelop.com/virrmd\">Space Convoy</a></li>\
            <li><a href=\"#url\">Twenty-Four</a></li>\
            <li><a href=\"#url\">Flight Academy</a></li>\
			<li><a href=\"#url\">Colony Defender</a></li>\
          </ul>\
        </li>\
		<li><a class=\"fly\" href=\"#url\" tabindex=\"1\">SUGames</a>\
          <ul class=\"flyout f3\">\
            <li><a href=\"cargobot\">Cargobot</a></li>\
            <li><a href=\"#url\">The Agency</a></li>\
            <li><a href=\"#url\">Dragon Street</a></li>\
			<li><a href=\"#url\">Bolts v Bones</a></li>\
          </ul>\
        </li>\
      </ul>\
    </li>\
\
    <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">3D Design</a>\
      <ul class=\"dropdown d3\">\
        <li><a href=\"#url\">Putin's Paradise</a></li>\
        <li><a href=\"tamblinthenvarblin\">Tamblin then Varblin</a></li>\
      </ul>\
    </li>\
\
    <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">About Us</a>\
      <ul class=\"dropdown d1\">\
        <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">John Morris</a>\
          <ul class=\"flyout f3\">\
            <li><a href=\"#url\">General</a></li>\
            <li><a href=\"winchellportfoliolist.html\">Portfolio</a></li>\
            <li><a href=\"#url\">Resume</a></li>\
          </ul>\
        </li>\
<!--\
		<li><a class=\"fly\" href=\"#url\" tabindex=\"1\">Contributor 2</a>\
          <ul class=\"flyout f4\">\
            <li><a href=\"#url\">General</a></li>\
            <li><a href=\"winchellportfoliolist.html\">Portfolio</a></li>\
            <li><a href=\"#url\">Resume</a></li>\
          </ul>\
        </li>\
        <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">Contributor 3</a>\
          <ul class=\"flyout f5\">\
            <li><a href=\"#url\">General</a></li>\
            <li><a href=\"#url\">Portfolio</a></li>\
            <li><a href=\"#url\">Resume</a></li>\
          </ul>\
        </li>\
-->\
      </ul>\
	</li>\
\
    <li><a class=\"fly\" href=\"#url\" tabindex=\"1\">Other Stuff</a>\
      <ul class=\"dropdown d4\">\
		<li><a href=\"tamblinthenvarblin\">Tamblin then Varblin</a></li>\
        <!--<li><a href=\"#url\" target=\"_blank\">The Puntin's Paradise Experience</a></li>-->\
        <!--<li><a href=\"#url\">What to do</a></li><li><a href=\"#url\">Places of interest</a></li><li><a href=\"#url\">National parks &amp; Museums</a></li>-->\
      </ul>\
    </li>\
\
  </ul>";
		
		
		
		document.getElementById("menu").innerHTML= menuStr;
	}

function toggleTag(tagId){
	var classType = document.getElementById(tagId).className;
	if(classType == "visible"){
		hideTag(tagId);	
	}else{
		showTag(tagId);	
	}
}
	
function showTag(tagId){
	document.getElementById(tagId).className = "visible";
	
}
function hideTag(tagId){
	document.getElementById(tagId).className = "invisible";
	
}
function loadTag(tagId){
	var boolId = tagId + "Loaded";
	var commentId = tagId + "Comment";
	var elementId = tagId + "Element";
	if(!unitCellLoaded){
		
		document.getElementById(elementId).innerHTML = unitCellUnityCanvas;
		unitCellLoaded = true;
	}
	toggleTag(tagId);
	toggleTag(commentId);
}
function unitCell(){
	
}