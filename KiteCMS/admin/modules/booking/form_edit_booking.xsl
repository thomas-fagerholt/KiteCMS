<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<xsl:variable name="selecteditem"><xsl:value-of select="//selecteditem"/></xsl:variable>

<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

<script type="text/javascript">
var dateReg = '^([0-2]?[0-9]|3[0-1])\-((0?[0-9]|1[0-2]))\-([0-9]{4})$';
var regex = new RegExp(dateReg);
function funcsubmit(){
	if(!document.booking.item[document.booking.item.selectedIndex].text.indexOf('--')==0){
		alert("<xsl:value-of select="util:localText('mustchooseresource')"/>");
		document.booking.item.focus();
		return false;
	}
	if(!regex.test(document.booking.date.value)){
		alert('<xsl:value-of select="util:localText('mustformatdate')"/>');
		document.booking.date.focus();
		return false;
	}
	if(!regex.test(document.booking.enddate.value))
		if(document.booking.returning.value!=''){
		alert('<xsl:value-of select="util:localText('mustformatenddate')"/>');
		document.booking.enddate.focus();
		return false;
	}
	if(document.booking.header.value==''){
		alert('<xsl:value-of select="util:localText('musttitle')"/>');
		document.booking.header.focus();
		return false;
	}
	return true;
}
</script>

<xsl:for-each select="//item[@pageid=//selectedpage]/booking[@id=$selecteditem]">
	<form method="post" name="booking" action="default.aspx">
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="bookingitem" value="{$selecteditem}"/>
	<input type="hidden" name="action" value="doedit"/>
	<table border="0">
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('resource')"/>:</td>
		<td>
			<select name="item" class="alminput">
			<option value=""><xsl:value-of select="util:localText('chooseresource')"/></option>
			<xsl:for-each select="//group">
			<xsl:sort select="@name"/>
				<option>
				<xsl:value-of select="@name"/>
				</option>
				<xsl:for-each select="item">
				<xsl:sort select="@name"/>
					<xsl:element name="option">
						<xsl:if test="booking/@id=$selecteditem">
						<xsl:attribute name="selected"></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@id"/></xsl:attribute>
						--><xsl:value-of select="@name"/>
					</xsl:element>
				</xsl:for-each>
			</xsl:for-each>
			</select>
		</td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('startdate')"/>:</td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">text</xsl:attribute>
				<xsl:attribute name="name">date</xsl:attribute>
				<xsl:attribute name="size">15</xsl:attribute>
				<xsl:attribute name="value">
					<xsl:call-template name="dateUS2DK">
					<xsl:with-param name="date">
					<xsl:if test="@returning=''">
						<xsl:value-of select="@date"/>
					</xsl:if>
					<xsl:if test="@returning!=''">
						<xsl:value-of select="@startdate"/>
					</xsl:if>
					</xsl:with-param>
					</xsl:call-template>
				</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td valign="middle" align="right"><xsl:value-of select="util:localText('repeat')"/>:</td>
		<td valign="top">
		<input type="hidden" name="returning" value="{@returning}"/>
	
		<xsl:choose>
		<xsl:when test="@returning='day'"><xsl:value-of select="util:localText('dayly')"/></xsl:when>
		<xsl:when test="@returning='week'"><xsl:value-of select="util:localText('weekly')"/></xsl:when>
		<xsl:otherwise>Ingen</xsl:otherwise>
		</xsl:choose>
	
		</td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('enddate')"/>:</td>
		<td>
			<xsl:element name="input">
				<xsl:attribute name="type">text</xsl:attribute>
				<xsl:attribute name="name">enddate</xsl:attribute>
				<xsl:attribute name="size">15</xsl:attribute>
				<xsl:attribute name="value">
					<xsl:if test="@returning!=''">
						<xsl:call-template name="dateUS2DK">
						<xsl:with-param name="date">
							<xsl:value-of select="@enddate"/>
						</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('starttime')"/>:</td>
		<td>
			<xsl:variable name="starttime"><xsl:value-of select="@starttime"/></xsl:variable>
			<select name="time" class="alminput">
			<xsl:for-each select="//time">
				<xsl:element name="option">
					<xsl:if test="@id=$starttime">
						<xsl:attribute name="selected"></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="value"><xsl:value-of select="@id"/></xsl:attribute>
					<xsl:value-of select="."/>
				</xsl:element>
			</xsl:for-each>
			</select>
		</td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('hourcount')"/>:</td>
		<td>
			<select name="timeLast" class="alminput" style="width:50;">
				<xsl:call-template name="timelast"><xsl:with-param name="hour">1</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">2</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">3</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">4</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">5</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">6</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">7</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">8</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">9</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">10</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">11</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">12</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">13</xsl:with-param></xsl:call-template>
				<xsl:call-template name="timelast"><xsl:with-param name="hour">14</xsl:with-param></xsl:call-template>
			</select>
		</td>
	</tr>
	<tr>
		<td valign="middle" align="right"><xsl:value-of select="util:localText('title')"/>:</td>
		<td valign="top"><input type="text" name="header" class="alminput" value="{header}"/></td>
	</tr>
	<tr>
		<td valign="middle" align="right"><xsl:value-of select="util:localText('text')"/>:</td>
		<td valign="top"><input type="text" name="text" size="35" class="alminput" value="{text}"/></td>
	</tr>
	<tr>
		<td></td>
		<td>
		<input type="submit" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();"/>&#160;
		<xsl:element name="input">
			<xsl:attribute name="type">button</xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="util:localText('delete')"/></xsl:attribute>
			<xsl:attribute name="class">almknap</xsl:attribute>
			<xsl:attribute name="onclick">if(confirm('<xsl:value-of select="util:localText('confirmdeletebooking')"/>')) {location.href='default.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=dodelete&amp;bookingitem=<xsl:value-of select="$selecteditem"/>&amp;item=<xsl:value-of select="//item[booking/@id=$selecteditem]/@id"/>'}</xsl:attribute>
		</xsl:element>&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/>
		</td>
	</tr>
	</table>
	</form>
</xsl:for-each>

</xsl:template>

<xsl:template name="dateUS2DK">
<xsl:param name='date'/>
	<xsl:value-of select="substring-before(substring-after($date,'/'),'/')"/>-<xsl:value-of select="substring-before($date,'/')"/>-<xsl:value-of select="substring-after(substring-after($date,'/'),'/')"/>
</xsl:template>

<xsl:template name="timelast">
<xsl:param name='hour'/>
<xsl:variable name="selecteditem"><xsl:value-of select="//selecteditem"/></xsl:variable>

	<xsl:element name="option">
		<xsl:if test="(@endtime - @starttime) = $hour">
			<xsl:attribute name="selected"/>
		</xsl:if>
	<xsl:value-of select="$hour"/>
	</xsl:element>
</xsl:template>

</xsl:stylesheet>