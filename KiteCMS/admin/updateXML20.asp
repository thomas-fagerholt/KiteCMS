<%

'##
'##	profilEdit version 2.0
'##
'##	Denne fil er til opdateringer for debatmodul fra v. 1.x til 2.0
'##

dim oXML
dim oXSL
Dim objNode
dim intValue
dim objNodeNewCdata

set oXML = server.createObject("MSXML2.DOMDocument")
set oXSL = server.createObject("MSXML2.DOMDocument")

oXML.async = false
oXSL.async = false

oXML.load(server.mapPath("/data/website.xml"))

	response.write "website XML loaded<br>Indsætning af CDATA i layout<br>"
	set objNodeRange = oXML.SelectNodes("//template/title")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).firstchild.firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			objNodeNewCdata.text = objNodeRange.item(counter).text
			objNodeRange.item(counter).text = ""
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		end if
	next
	set objNodeRange = oXML.SelectNodes("//template/publicurl")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).firstchild.firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			objNodeNewCdata.text = objNodeRange.item(counter).text
			objNodeRange.item(counter).text = ""
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		end if
	next
	set objNodeRange = oXML.SelectNodes("//template/adminurl")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).firstchild.firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			objNodeNewCdata.text = objNodeRange.item(counter).text
			objNodeRange.item(counter).text = ""
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		end if
	next
	set objNodeRange = oXML.SelectNodes("//template/xslurl")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).firstchild.firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			objNodeNewCdata.text = objNodeRange.item(counter).text
			objNodeRange.item(counter).text = ""
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		end if
	next

	set objNodeRange = oXML.SelectNodes("//template/templatecolor")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		elseif objNodeRange.item(counter).firstchild.firstchild is nothing then
			set objNodeNewCdata = oXML.createNode("cdatasection","","")
			objNodeNewCdata.text = objNodeRange.item(counter).text
			objNodeRange.item(counter).text = ""
			set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
		end if
	next

	response.write "Indsætning af updated-attribut i page<br>"
	set objNodeRange = oXML.SelectNodes("//page")
	for counter = 0 to objNodeRange.length-1
		if objNodeRange.item(counter).getAttributenode("updated") is nothing then
			objNodeRange.item(counter).SetAttribute "updated",""
		end if
	next

	'## Gem XML
	response.write "XML gemmes<br>"
	oXML.save(server.mapPath("/data/website.xml"))
	Application("oXMLWebsite") = oXML.xml

	dim fso
	set fso=Server.CreateObject("Scripting.FileSystemObject")
	if fso.FileExists (server.mapPath("/data/email.xml")) then

		oXML.load(server.mapPath("/data/email.xml"))

		response.write "email XML loaded<br>opdatering af dokument<br>"
		set objNodeRange = oXML.SelectNodes("//subscribe")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).getAttributenode("active") is nothing then
				objNodeRange.item(counter).SetAttribute "active","1"
			end if
		next

		set objNodeRange = oXML.SelectNodes("//emailcategory")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).SelectsingleNode("archive") is nothing then
				set objNode = oXML.createNode("element","archive","")
				objNode.text = "0"
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNode)
			end if

			if objNodeRange.item(counter).SelectsingleNode("title") is nothing then
				set objNode = oXML.createNode("element","title","")
				objNode.text = "category "& objNodeRange.item(counter).getAttributenode("id").Value
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNode)
			end if
			if not objNodeRange.item(counter).SelectsingleNode("onIsSubscribe") is nothing then
				set objNodeTemp = objNodeRange.item(counter).removeChild(objNodeRange.item(counter).SelectsingleNode("onIsSubscribe"))
			end if
			if not objNodeRange.item(counter).SelectsingleNode("subject") is nothing then
				set objNodeTemp = objNodeRange.item(counter).removeChild(objNodeRange.item(counter).SelectsingleNode("subject"))
			end if
			if objNodeRange.item(counter).getAttributenode("pageid") is nothing then
				objNodeRange.item(counter).SetAttribute "pageid","0"
				response.write "pageid for emailcategory skal rettes i hånden<br>"
			end if
		next

		set objNodeRange = oXML.SelectNodes("//onSubscribe")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).firstchild.firstchild is nothing then
				set objNodeNewCdata = oXML.createNode("cdatasection","","")
				objNodeNewCdata.text = objNodeRange.item(counter).text
				objNodeRange.item(counter).text = ""
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
			end if
		next
		set objNodeRange = oXML.SelectNodes("//onUnSubscribe")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).firstchild.firstchild is nothing then
				set objNodeNewCdata = oXML.createNode("cdatasection","","")
				objNodeNewCdata.text = objNodeRange.item(counter).text
				objNodeRange.item(counter).text = ""
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
			end if
		next
		set objNodeRange = oXML.SelectNodes("//subject")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).firstchild.firstchild is nothing then
				set objNodeNewCdata = oXML.createNode("cdatasection","","")
				objNodeNewCdata.text = objNodeRange.item(counter).text
				objNodeRange.item(counter).text = ""
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
			end if
		next
		set objNodeRange = oXML.SelectNodes("//header")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).firstchild.firstchild is nothing then
				set objNodeNewCdata = oXML.createNode("cdatasection","","")
				objNodeNewCdata.text = objNodeRange.item(counter).text
				objNodeRange.item(counter).text = ""
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
			end if
		next
		set objNodeRange = oXML.SelectNodes("//footer")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).firstchild.firstchild is nothing then
				set objNodeNewCdata = oXML.createNode("cdatasection","","")
				objNodeNewCdata.text = objNodeRange.item(counter).text
				objNodeRange.item(counter).text = ""
				set objNodeTemp = objNodeRange.item(counter).appendChild(objNodeNewCdata)
			end if
		next
		'## Gem XML

		response.write "XML gemmes<br>"
		oXML.save(server.mapPath("/data/email.xml"))
	end if

	set fso=Server.CreateObject("Scripting.FileSystemObject")
	if fso.FileExists (server.mapPath("/data/booking.xml")) then

		oXML.load(server.mapPath("/data/booking.xml"))

		response.write "booking XML loaded<br>opdatering af dokument<br>"

		set objNodeRange = oXML.SelectNodes("//item")
		for counter = 0 to objNodeRange.length-1
			if objNodeRange.item(counter).getAttributenode("pageid") is nothing then
				objNodeRange.item(counter).SetAttribute "pageid","0"
				response.write "pageid for ressourcer skal rettes i hånden<br>"
			end if
		next

		response.write "XML gemmes<br>"
		oXML.save(server.mapPath("/data/booking.xml"))
	end if

	oXML.load(server.mapPath("/admin/data/access.xml"))

	response.write "access XML loaded<br>Indsætning nye noder<br>"
	set objNode = oXML.SelectSingleNode("//method[@id='2104']")
	if objNode is nothing then
		set objNode = oXML.SelectSingleNode("//module[@id='2100']")
		set objNodeNewNode = oXML.createNode("element","method","")
		set objNodeTemp = objNode.appendChild(objNodeNewNode)
		objNodeNewNode.SetAttribute "id","2104"
		objNodeNewNode.SetAttribute "name","addcategory"
		objNodeNewNode.SetAttribute "cache","none"
	end if
	set objNode = oXML.SelectSingleNode("//method[@id='2105']")
	if objNode is nothing then
		set objNode = oXML.SelectSingleNode("//module[@id='2100']")
		set objNodeNewNode = oXML.createNode("element","method","")
		set objNodeTemp = objNode.appendChild(objNodeNewNode)
		objNodeNewNode.SetAttribute "id","2105"
		objNodeNewNode.SetAttribute "name","editarchive"
		objNodeNewNode.SetAttribute "cache","none"
	end if
	oXML.save(server.mapPath("/admin/data/access.xml"))
%>