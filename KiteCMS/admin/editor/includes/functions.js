var isIE = (navigator.appVersion.indexOf("MSIE") != -1);

function funcSetStyle(style){
	var bookmark;
	if (isIE)
	{
		if(document.selection.type=="Text")
	  		bookmark = document.selection.createRange().getBookmark();
		document.execCommand('formatBlock',false,'<address>')
		var text = document.getElementById(currentcontentholder).innerHTML;
	}
	else
	{
        if(document.getElementById(currentcontentholder).contentWindow.getSelection()=='')
            return;
		document.getElementById(currentcontentholder).contentWindow.document.execCommand('formatBlock',false,'<address>')
		var text = document.getElementById(currentcontentholder).contentWindow.document.body.innerHTML;
	}	

	if (style.toUpperCase() =='P' || style.toUpperCase() =='H1' || style.toUpperCase() =='H2' || style.toUpperCase() =='H3' || style.toUpperCase() =='H4' || style.toUpperCase() =='H5' || style.toUpperCase() =='H6')
	{
		text = text.replace(/<address[^<>]*>/gi, "<"+ style +">");
		text = text.replace(/<\/address>/gi, "</"+ style +">");
	}
	else
	{
		text = text.replace(/<address[^<>]*>/gi, "<p class='"+ style +"'>");
		text = text.replace(/<\/address>/gi, "</p>");
	}
	
	if (isIE)
	{
	    if (bookmark != null) {
		    document.getElementById(currentcontentholder).innerHTML = text;
		    focusCurrentContentholder();
		    var sel = document.body.createTextRange();
		    sel.moveToBookmark(bookmark);
		    sel.select();
        }
	}
	else
	{
   		document.getElementById(currentcontentholder).contentWindow.document.body.innerHTML = text;
	}
	
}

function funcBold(){
    if (isIE)
        document.execCommand('Bold',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('Bold',false,null);
}

function funcItalic(){
    if (isIE)
        document.execCommand('Italic',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('Italic',false,null);
}

function funcUnderline(){
    if (isIE)
        document.execCommand('Underline',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('Underline',false,null);
}

function funcJustifyLeft(){
    if (isIE)
        document.execCommand('JustifyLeft',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('JustifyLeft',false,null);
}

function funcJustifyCenter(){
    if (isIE)
        document.execCommand('JustifyCenter',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('JustifyCenter',false,null);
}

function funcJustifyRight(){
    if (isIE)
        document.execCommand('JustifyRight',false,null);
    else
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('JustifyRight',false,null);
}

function funcUnorderedlist(){
    if (isIE) {
        focusCurrentContentholder();
        document.execCommand('insertunorderedlist',false,null);
    } else {
        document.getElementById(currentcontentholder).contentWindow.document.execCommand('insertunorderedlist',false,null);
    }
}

function funcColor(){
	if (isIE) {
		var arr = showModalDialog( "/admin/editor/includes/selcolor.aspx","","font-family:Verdana; font-size:12; dialogWidth:300px; dialogHeight:400px;status:no;help:no" );
		if (arr != null)
			document.execCommand("foreColor",false, arr);
	} else {
		winModalWindow = window.open ("/admin/editor/includes/selcolor.aspx","ModalChild","dependent=yes,width=300,height=400")
		winModalWindow.focus();
	}		
}

function funcColorSelected(color){
	document.getElementById(currentcontentholder).contentWindow.document.execCommand("foreColor", false, color);
}

function funcImage(){
	if (isIE) {
		var arr = showModalDialog("/admin/editor/includes/image.html","","font-family:Verdana; font-size:10; dialogWidth:700px; dialogHeight:550px;status:no;help:no" );
        if (arr) {
		    if (arr["src"]) {
			    if(arr["src"] !='') {
				    focusCurrentContentholder();
				    var sel = document.selection.createRange();
				    if (arr["align"] != '')
					    sel.pasteHTML( '<img src="' + arr["src"] + '" alt="' + arr["alt"] + '" title="' + arr["title"] + '" align="'+ arr["align"] +'">' );
				    else
					    sel.pasteHTML( '<img src="' + arr["src"] + '" alt="' + arr["alt"] + '" title="' + arr["title"] + '">' );
			    }
		    }
		}
	} else {
		winModalWindow = window.open ("/admin/editor/includes/image.html","ModalChild","dependent=yes,width=700px,height=550px")
		winModalWindow.focus();
	}
}

function funcImageSelected(img,alt,title){
	elem = document.getElementById(currentcontentholder).contentWindow.document.createElement("img");
	elem.setAttribute("src", img);
	elem.setAttribute("alt", alt);
	elem.setAttribute("title", title);

	insertNodeAtSelection(document.getElementById(currentcontentholder).contentWindow, elem);

}

function funcMedia(){
	if (isIE) {
	    sel = document.selection.createRange();
	    if(sel.htmlText != ""){
		    var arr = showModalDialog("/admin/editor/includes/media.html", "", "font-family:Verdana; font-size:10; dialogWidth:600px; dialogHeight:500px;status:no;help:no" );
		    if (arr != null){
			    if (arr["href"] != '') {
				    if(document.selection.type == "Control"){
					    if(sel.item(0).tagName == "IMG"){
						    document.execCommand('createlink',false,arr["href"]);
						    if (arr["target"] !=''){
							    newLinkId = sel.item(0).parentElement.sourceIndex;
							    document.all(newLinkId).setAttribute("target",arr["target"]);
							    document.all(newLinkId).setAttribute("title",arr["titletag"]);
						    }
					    }
				    } else {
					    if (arr["target"] !='')
						    sel.pasteHTML("<A href='"+ arr["href"] +"' target='"+ arr["target"] +"' title='"+ arr["titletag"] +"'>"+ sel.htmlText +"</a>");
					    else			
						    sel.pasteHTML("<A href='"+ arr["href"] +"' title='"+ arr["titletag"] +"'>"+ sel.htmlText +"</a>");
				    }
			    }
		    }
	    }else
		    alert(chooseText);
	} else {
		winModalWindow = window.open ("/admin/editor/includes/media.html","ModalChild","dependent=yes,width=600,height=500")
		winModalWindow.focus();
	}
}

function funcLink(){
	if (isIE){
		focusCurrentContentholder();
		var arrAnchors = new Array();
		sel = document.selection.createRange();
		if(sel.htmlText != ""){
			for (var i = 0; i < document.getElementById(currentcontentholder).getElementsByTagName("a").length; i++)
				if (document.getElementById(currentcontentholder).getElementsByTagName("a")[i].name!="")
					arrAnchors[i]=document.getElementById(currentcontentholder).getElementsByTagName("a")[i].name;

			var arr = showModalDialog("/admin/editor/includes/sellink.aspx", arrAnchors, "font-family:Verdana; font-size:10; dialogWidth:350px; dialogHeight:550px;status:no;help:no" );
			if (arr != null){
				if (arr["href"] != '') {
					if(document.selection.type == "Control"){
						if(sel.item(0).tagName == "IMG"){
							document.execCommand('createlink',false,arr["href"]);
							if (arr["target"] !=''){
								newLinkId = sel.item(0).parentElement.sourceIndex;
								document.all(newLinkId).setAttribute("target",arr["target"]);
							}
						}
					} else {
						var titleString = "";
						var targetString = "";
						var insertString;
						var selHtml = sel.htmlText;
						selHtml = selHtml.replace(/(<a[^<]*>)((.|[\r\n])*)(<\/a>)/gi, "$2"); //Replace existing link
						if (arr["title"] !='')
							titleString =" title='"+ arr["title"] +"'";
						if (arr["target"] !='')
							targetString =" target='"+ arr["target"] +"'";

						insertString ="<a href='"+ arr["href"] +"'"+ targetString + titleString +">"+ selHtml +"</a>";
						insertString = insertString.replace(/(<a [^>]*>)(?:\r\n)*(<(p|h[1-5])[^>]*>)((?:.(?!\/(?:p|h[1-5])>))*)<\/\3>(?:\r\n)*<\/a>/gi, "$2$1$4</a></$3>"); //Move link inside blockelement

						sel.pasteHTML(insertString);
					}
				}
			}
		}else
			alert(chooseText);
	} else {
		var strAnchors = "";
		for (var i = 0; i < document.getElementById(currentcontentholder).contentWindow.document.body.getElementsByTagName("a").length; i++) {
			if (document.getElementById(currentcontentholder).contentWindow.document.body.getElementsByTagName("a")[i].name!="") {
				strAnchors+= document.getElementById(currentcontentholder).contentWindow.document.body.getElementsByTagName("a")[i].name +",";
			}
		}
		winModalWindow = window.open("/admin/editor/includes/sellink.aspx?anchors="+ strAnchors ,"ModalChild","dependent=yes,width=350,height=550");
		winModalWindow.focus();
	}
}
function funcLinkSelected(href,target,title){
	document.getElementById(currentcontentholder).contentWindow.document.execCommand("CreateLink",false,'__temporary__')
	var linkelements = document.getElementById(currentcontentholder).contentWindow.document.getElementsByTagName('A');
	for (var counter=0;counter < linkelements.length; counter ++)
	{
		if (linkelements[counter].hasAttribute('href')) {
	     if (linkelements[counter].getAttribute('href').search('__temporary__') != -1) {
	   		linkelements[counter].setAttribute('href',href);
			if (target != "")
				linkelements[counter].setAttribute("target", target);
			if (title != "")
				linkelements[counter].setAttribute("title", title);
			}
		}
	}
}
function funcAnchor(){
	if (isIE) {
		var arr = showModalDialog( "/admin/editor/includes/anchor.aspx","","font-family:Verdana; font-size:12; dialogWidth:300px; dialogHeight:150px;status:no;help:no" );
		if (arr != null) {
			if(document.selection.createRange().text == ""){
    			focusCurrentContentholder();
				document.selection.createRange().pasteHTML("<A name='"+ arr +"'></a>");
			}else{
				var selHtml = document.selection.createRange().htmlText;
				document.selection.createRange().pasteHTML("<A name='"+ arr +"'>"+ selHtml +"</a>");
			}
		}
	} else {
		winModalWindow = window.open ("/admin/editor/includes/anchor.aspx","ModalChild","dependent=yes,width=250,height=150")
		winModalWindow.focus();

	}
}
function funcAnchorSelected(name){
	anchor = document.getElementById(currentcontentholder).contentWindow.document.createElement("a");
	anchor.innerHTML = document.getElementById(currentcontentholder).contentWindow.getSelection(); 
	anchor.setAttribute("name", name);
	insertNodeAtSelection(document.getElementById(currentcontentholder).contentWindow, anchor);
}

function funcAbbr(){
	if (isIE) {
    sel = document.selection.createRange();
    if(sel.htmlText != ""){
			var arr = showModalDialog( "/admin/editor/includes/abbr.aspx","","font-family:Verdana; font-size:12; dialogWidth:400px; dialogHeight:150px;status:no;help:no" );
			if (arr != null) 
				if (arr["value"] != null) {
					var selHtml = document.selection.createRange().htmlText;
					document.selection.createRange().pasteHTML("<"+ arr["type"] +" title='"+ arr["value"] +"'>"+ selHtml +"</"+ arr["type"] +">");
			}
	    }else
		    alert(chooseText);
	} else {
		winModalWindow = window.open ("/admin/editor/includes/abbr.aspx","ModalChild","dependent=yes,width=350,height=150")
		winModalWindow.focus();
	}
}

function funcAbbrSelected(title, type){
	anchor = document.getElementById(currentcontentholder).contentWindow.document.createElement(type);
	anchor.innerHTML = document.getElementById(currentcontentholder).contentWindow.getSelection(); 
	anchor.setAttribute("title", title);
	insertNodeAtSelection(document.getElementById(currentcontentholder).contentWindow, anchor);
}

function funcPasteWord(message){
  if (isIE) {
	focusCurrentContentholder();
	var curAe = document.selection.createRange();
	document.getElementById(currentcontentholder +'_hidden').innerHTML = '';
	document.getElementById(currentcontentholder +'_hidden').focus();
	document.execCommand('paste');

	var text = document.getElementById(currentcontentholder +'_hidden').innerHTML;

	if(message!='' && (text.search(/class=mso/gi) != -1 || text.search(/mso-/gi) != -1))
		alert(message);
	
	text = text.replace(/<(?!BR\b|\/?P\b|\/?ol\b|\/?h[1-6]\b|\/?ul\b|\/?li\b).[^<>]*>/gi, '');
	text = text.replace(/<(\S+) .[^<>]*>/g, '<$1>');
	
	document.getElementById(currentcontentholder +'_hidden').innerHTML = text;
	focusCurrentContentholder();
	curAe.pasteHTML(document.getElementById(currentcontentholder +'_hidden').innerHTML);
	document.getElementById(currentcontentholder +'_hidden').innerHTML = '';

	return false;
  }
  else
  {
    winModalWindow = window.open ("/admin/editor/includes/pastefromword.aspx","ModalChild","dependent=yes,width=300,height=400")
    winModalWindow.focus();
  }
}

function pastetext(text)
{
   document.getElementById(currentcontentholder).contentWindow.document.execCommand('inserthtml',false,text.replace('\n','<br>'));
}

function MENU_FILE_SAVE_onclick() {

	if (blnBorder==1)
		funcToggleBorder();
    for(i=0; i<allEditors.length - 1; i++) {
	    if (strMode['tbContent_'+ allEditors[i]] == 'text'){
		    if (isIE)
			    strContent = document.getElementById('tbContent_'+ allEditors[i]).innerText;
		    else {
			    var html = document.getElementById('tbContent_'+ allEditors[i]).contentWindow.document.body.ownerDocument.createRange();
			    html.selectNodeContents(document.getElementById('tbContent_'+ allEditors[i]).contentWindow.document.body);
			    strContent = html.toString();
		    }
        } else {
		    if (isIE){
			    strContent = document.getElementById('tbContent_'+ allEditors[i]).innerHTML;
		    } else {
			    strContent = document.getElementById('tbContent_'+ allEditors[i]).contentWindow.document.body.innerHTML;
		    }
	    }
	    document.getElementById('tbContent_'+ allEditors[i] + '_temp').value = cleanUpWordCode(strContent);
    }
    if (typeof(funcUpdateFields) != 'undefined')
        funcUpdateFields();
    else
        document.tempForm[0].submit();
}

function cleanUpWordCode(html){
	html = html.replace(/<o:p><\/o:p>/gi, ""); // Remove all instances of <o:p></o:p>
	html = html.replace(/<st1:.*?>/gi, ""); // delete all smarttags
	html = html.replace(/<(\?xml)[^<>]*\/>/gi, ""); // delete all <?xml tags
	return html;
}

function InsertTable() {
 if (isIE) {
  var args = new Array();
  var arr = null;
     
  arr = null;
    
  arr = showModalDialog( "/admin/editor/includes/instable.aspx",args,"font-family:Verdana; font-size:12; dialogWidth:500px; dialogHeight:500px;status:no;help:no");
  if(arr != null)
	{
	  if (arr["tablehtml"]!=""){
	  	focusCurrentContentholder();
		var curAe = document.selection.createRange();
		curAe.pasteHTML(arr["tablehtml"]);
	  }else
	  if (arr["NumRows"]!="" && arr["NumCols"]!="") {
	
		var trObj = new Array();
		var tdObj = new Array();
	
		var rows= arr["NumRows"];
		var cols= arr["NumCols"];
		
		tableObj = document.createElement("TABLE");
	
		if (arr["cellpad"]!="") { tableObj.cellpadding = arr["cellpad"];}
		if (arr["cellspace"]!="") { tableObj.cellspacing = arr["cellspace"];}
		if (arr["tablesummary"]!="") { tableObj.summary = arr["tablesummary"];}
		if (arr["tableborder"]!="") {
			if (arr["tableborder"]==0){
				tableObj.border = "0";
			} else
				tableObj.border = arr["tableborder"];
		}
		if (arr["tablecaption"]!="") {
			tcaptionObj = document.createElement("caption");
			tcaptionObj.innerHTML = arr["tablecaption"];
			tableObj.appendChild(tcaptionObj);
		}
	
		tbodyObj = document.createElement("TBODY");
	
		trObj[0] = document.createElement("TR");
		for(i=0;i<cols;i++){
	 		tdObj[i] = document.createElement("TD");
	 		tdObj[i].innerHTML = '';
	  		trObj[0].appendChild(tdObj[i]);
		}
		for(i=0;i<rows-1;i++)trObj[i+1] = trObj[0].cloneNode(true);
	
		for(i=0;i<rows;i++)tbodyObj.appendChild(trObj[i]);
		tableObj.appendChild(tbodyObj);
	
		focusCurrentContentholder();
		var curAe = document.selection.createRange();
		curAe.pasteHTML(curAe.htmlText + tableObj.outerHTML);
	    }
	}
 } else {
	winModalWindow = window.open ("/admin/editor/includes/instable.aspx","ModalChild","dependent=yes,width=500,height=500")
	winModalWindow.focus();
 }   
}

function funcInsertTableSelected(rows,cols,padding,spacing,border,html,summary,caption){
	if (html!=""){
		table = document.getElementById(currentcontentholder).contentWindow.document.createElement("span");
		table.innerHTML = html; 
		insertNodeAtSelection(document.getElementById(currentcontentholder).contentWindow, table);
	} else {
		var trObj = new Array();
		var tdObj = new Array();
	
		var rows= rows;
		var cols= cols;
		
		tableObj = document.getElementById(currentcontentholder).contentWindow.document.createElement("TABLE");
	
		if (padding!="") { tableObj.setAttribute("cellpadding", padding);}
		if (spacing!="") { tableObj.setAttribute("cellspacing", spacing);}
		if (summary!="") { tableObj.setAttribute("summary", summary);}
		if (border!="") {
			if (border==0){
				tableObj.border = "0";
			} else
				tableObj.border = border;
		}
	
		tbodyObj = document.getElementById(currentcontentholder).contentWindow.document.createElement("TBODY");
		tcaptionObj = document.getElementById(currentcontentholder).contentWindow.document.createElement("CAPTION");
		tcaptionObj.innerHTML = caption;
		trObj[0] = document.getElementById(currentcontentholder).contentWindow.document.createElement("TR");
		for(i=0;i<cols;i++){
	 		tdObj[i] = document.getElementById(currentcontentholder).contentWindow.document.createElement("TD");
	 		tdObj[i].innerHTML = '&nbsp;';
	  		trObj[0].appendChild(tdObj[i]);
		}
		for(i=0;i<rows-1;i++)trObj[i+1] = trObj[0].cloneNode(true);
	
		for(i=0;i<rows;i++)tbodyObj.appendChild(trObj[i]);
		tableObj.appendChild(tcaptionObj);
		tableObj.appendChild(tbodyObj);

		insertNodeAtSelection(document.getElementById(currentcontentholder).contentWindow, tableObj);
	
	}
}

function InsertTableRow(){
	if (isIE){
		focusCurrentContentholder();
		var index = -1;
		var selSelection = document.selection;
		var rngRange = selSelection.createRange();
		elElement = rngRange.parentElement() ;
		while (elElement != null && elElement.tagName != "TABLE" && elElement.id != "tbContent") {
			if(elElement.tagName == "TR")
				index = elElement.rowIndex+1;
			elElement = elElement.parentElement ;
		}
	}
	if (elElement != null){
		if (elElement && elElement.rows && elElement.insertRow) {
			var tr = elElement.insertRow(index);
			for (var i = 0; i < elElement.rows[0].cells.length; i ++)
			{
				tr.appendChild(elElement.rows[index-1].cells[i].cloneNode(false));
			}
		}
	}
}

function funcToggleBorder(){
	if(isIE)
	{
	    for(i=0; i<allEditors.length - 1; i++) {
		if (blnBorder==0)
			document.getElementById('tbContent_'+ allEditors[i]).className += ' visibleborders';	
		else
			document.getElementById('tbContent_'+ allEditors[i]).className = document.getElementById('tbContent_'+ allEditors[i]).className.replace(' visibleborders','');	
			}
	} else {
		if (blnBorder==0)
			document.getElementById(currentcontentholder).contentWindow.document.body.className = 'visibleborders';	
		else
			document.getElementById(currentcontentholder).contentWindow.document.body.className = '';	
		}
		blnBorder=Math.abs(blnBorder-1);
}


function funcUnlink(){
    if (isIE) {
	    focusCurrentContentholder();
		document.execCommand('unlink',false,null);
	 } else {
         document.getElementById(currentcontentholder).contentWindow.document.execCommand('Unlink',false,null);
	 }
	 
}

function funcToggleMode(){
	if (strMode[currentcontentholder] == 'html'){
		if (isIE) {
			document.getElementById(currentcontentholder).innerText = document.getElementById(currentcontentholder).innerHTML;
		} else {
			var html = document.createTextNode(document.getElementById(currentcontentholder).contentWindow.document.body.innerHTML);
			document.getElementById(currentcontentholder).contentWindow.document.body.innerHTML = "";
    			document.getElementById(currentcontentholder).contentWindow.document.body.appendChild(html);
		}
		
		strMode[currentcontentholder] = 'text';
/*
		var objects = document.getElementsByTagName("DIV");
		for( i = 0; i < objects.length; i++ ) {
			if(objects[i].id!='divSave' && objects[i].id!='divCancel' && objects[i].id!='divHidden' && objects[i].id!='divText' && objects[i].className=='tbButton'){
				objects[i].disabled=true;
				objects[i].style.visibility = "hidden";
			}
		}
		document.getElementById('selectClass').disabled=true;
*/
  	}else{
  		// check if any script or form-tags
		blnEdit[currentcontentholder] = true;
		if (isIE){
			if(document.getElementById(currentcontentholder).innerText.toLowerCase().indexOf('<script')>-1 ||document.getElementById(currentcontentholder).innerText.toLowerCase().indexOf('<form')>-1 || document.getElementById(currentcontentholder).innerText.toLowerCase().indexOf('<object')>-1)
				blnEdit[currentcontentholder] = false;		
		} else {
			var html = document.getElementById(currentcontentholder).contentWindow.document.body.ownerDocument.createRange();
			html.selectNodeContents(document.getElementById(currentcontentholder).contentWindow.document.body);
			if(html.toString().toLowerCase().indexOf('<script')>-1 || html.toString().toLowerCase().indexOf('<form')>-1 || html.toString().toLowerCase().indexOf('<object')>-1)
				blnEdit[currentcontentholder] = false;		
		}
  		
		if (blnEdit[currentcontentholder])
		{
/*			var objects = document.getElementsByTagName("DIV");
			for( i = 0; i < objects.length; i++ ) {
				if(objects[i].id!='divSave' && objects[i].id!='divCancel' && objects[i].id!='divHidden' && objects[i].id!='divText' && objects[i].id!='warning'){
					objects[i].style.visibility = "visible";
					objects[i].disabled=false;
				}
			}
			document.getElementById('selectClass').disabled=false;
*/	
			if (isIE){
				document.getElementById(currentcontentholder).innerHTML = document.getElementById(currentcontentholder).innerText;
			} else {
				var html = document.getElementById(currentcontentholder).contentWindow.document.body.ownerDocument.createRange();
				html.selectNodeContents(document.getElementById(currentcontentholder).contentWindow.document.body);
				document.getElementById(currentcontentholder).contentWindow.document.body.innerHTML = html.toString();
			}
			strMode[currentcontentholder] = 'html';
		}
		else
			alert(hasjavascriptText);
	}
}

  function insertNodeAtSelection(win, insertNode)
  {
      var sel;
      // get current selection
      sel = win.getSelection();

      // get the first range of the selection
      // (there's almost always only one range)
      var range = sel.getRangeAt(0);

      // deselect everything
      sel.removeAllRanges();

      // remove content of current selection from document
      range.deleteContents();

      // get location of current selection
      var container = range.startContainer;
      var pos = range.startOffset;

      // make a new range for the new selection
      range=document.createRange();

      if (container.nodeType==3 && insertNode.nodeType==3) {

        // if we insert text in a textnode, do optimized insertion
        container.insertData(pos, insertNode.nodeValue);

        // put cursor after inserted text
        range.setEnd(container, pos+insertNode.length);
        range.setStart(container, pos+insertNode.length);

      } else {


        var afterNode;
        if (container.nodeType==3) {

          // when inserting into a textnode
          // we create 2 new textnodes
          // and put the insertNode in between

          var textNode = container;
          container = textNode.parentNode;
          var text = textNode.nodeValue;

          // text before the split
          var textBefore = text.substr(0,pos);
          // text after the split
          var textAfter = text.substr(pos);

          var beforeNode = document.createTextNode(textBefore);
          var afterNode = document.createTextNode(textAfter);

          // insert the 3 new nodes before the old one
          container.insertBefore(afterNode, textNode);
          container.insertBefore(insertNode, afterNode);
          container.insertBefore(beforeNode, insertNode);

          // remove the old node
          container.removeChild(textNode);

        } else {

          // else simply insert the node
          afterNode = container.childNodes[pos];
          container.insertBefore(insertNode, afterNode);
        }

        range.setEnd(afterNode, 0);
        range.setStart(afterNode, 0);
      }

      sel.addRange(range);
  }
  
function button_over(eButton){
	eButton.style.backgroundColor = "#ffffff";
}
function button_out(eButton){
	eButton.style.backgroundColor = "transparent";
}

function focusCurrentContentholder(){
	if (currentcontentholder!= undefined)
		if (document.getElementById(currentcontentholder))
			document.getElementById(currentcontentholder).focus();
}

function CheckForEditCurrentcontentholder(){
    if (currentcontentholder == '') {
        alert(choosecontentholderText);
        return false;
    }
    return true;
}
function CheckForEditMode(){
    CheckForEditCurrentcontentholder();
    if (strMode[currentcontentholder] == 'text') {
		alert(notintextmodeText);
        return false;
    } else {
        return true;
    }
}

function FireFoxFocus(evt){
	elElement = evt.target;
	while (elElement.parentNode != null)
		elElement = elElement.parentNode ;
	if (elElement.getElementsByTagName('body')[0] != null)
        currentcontentholder='tbContent_' + elElement.getElementsByTagName('body')[0].id.replace('nsbody_','');
}

var gl_fieldname;
function funcImagePopup(fieldname){
	gl_fieldname = fieldname;
	if (isIE) {
		var arr = showModalDialog("/admin/editor/includes/imagePopup.html", "", "font-family:Verdana; font-size:10; dialogWidth:700px; dialogHeight:600px;status:no;help:no" );
		if (arr != null){
			if (arr["src"] != '') {
				document.getElementById(fieldname).value = arr["src"];
				document.getElementById("img_" + fieldname).src = arr["src"];
			}
		}
	} else {
		winModalWindow = window.open("/admin/editor/includes/imagePopup.html","ModalChild","dependent=yes,width=700,height=600")
		winModalWindow.focus();
	}
}

function funcImagePopupSelected(img){
	document.getElementById(gl_fieldname).value= img;
	document.getElementById("img_" + gl_fieldname).src = img;
	parent.window.close();
}

function funcUnescapeUnicode(str) {
    var r = /\\u([\d\w]{4})/gi;
    str = str.replace(r, function (match, grp) {
        return String.fromCharCode(parseInt(grp, 16));
    });
    return str;
}
