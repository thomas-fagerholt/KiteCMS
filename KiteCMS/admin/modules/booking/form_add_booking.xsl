<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="time" select="-1" />
<xsl:param name="date" select="-1" />

<xsl:template match="/">
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>
<script type="text/javascript">
var dateReg = '^([0-2]?[0-9]|3[0-1])\-((0?[0-9]|1[0-2]))\-([0-9]{4})$';
var regex = new RegExp(dateReg);
function funcsubmit(){
	if(!document.booking.item[document.booking.item.selectedIndex].text.indexOf('--')==0){
		alert('<xsl:value-of select="util:localText('mustchooseresource')"/>');
		document.booking.item.focus();
		return false;
	}
	if(!regex.test(document.booking.date.value)){
		alert('<xsl:value-of select="util:localText('mustformatdate')"/>');
		document.booking.date.focus();
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

<form method="post" name="booking" action="default.aspx">
<input type="hidden" name="pageid" value="{$selectedpage}"/>
<input type="hidden" name="action" value="doadd"/>

<table border="0">
<tr>
	<td valign="top" align="right"><xsl:value-of select="util:localText('resource')"/>:</td>
	<td>
		<select name="item" class="alminput">
		<option value=""><xsl:value-of select="util:localText('chooseresource')"/></option>
		<xsl:for-each select="//group[item/@pageid=$selectedpage]">
		<xsl:sort select="@name"/>
			<option>
			<xsl:value-of select="@name"/>
			</option>
			<xsl:for-each select="item[@pageid=$selectedpage]">
			<xsl:sort select="@name"/>
				<xsl:element name="option">
					<xsl:if test="//selecteditem=@id">
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
	<td valign="top" align="right"><xsl:value-of select="util:localText('bookingdate')"/>:</td>
	<td>
		<xsl:if test="$date!=-1">
		<input type="text" name="date" value="{$date}" size="15" maxlenght="50" class="alminput"/>
		</xsl:if>
		<xsl:if test="$date=-1">
		<input type="text" name="date" value="" size="15" maxlenght="50" class="alminput"/>
		</xsl:if>
	</td>
</tr>
<tr>
	<td valign="top" align="right"><xsl:value-of select="util:localText('starttime')"/>:</td>
	<td>
		<select name="time" class="alminput">
		<xsl:for-each select="//time">
			<xsl:element name="option">
				<xsl:if test="@id=$time">
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
			<option><xsl:value-of select="1"/></option>
			<option><xsl:value-of select="2"/></option>
			<option><xsl:value-of select="3"/></option>
			<option><xsl:value-of select="4"/></option>
			<option><xsl:value-of select="5"/></option>
			<option><xsl:value-of select="6"/></option>
			<option><xsl:value-of select="7"/></option>
			<option><xsl:value-of select="8"/></option>
		</select>
	</td>
</tr>
<tr>
	<td valign="middle" align="right"><xsl:value-of select="util:localText('title')"/>:</td>
	<td><input type="text" name="header" value="" size="35" maxlenght="50" class="alminput"/></td>
</tr>
<tr>
	<td valign="middle" align="right"><xsl:value-of select="util:localText('text')"/>:</td>
	<td><input type="text" name="text" value="" size="35" maxlenght="50" class="alminput"/></td>
</tr>
<tr>
	<td></td>
	<td>
	<input type="submit" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();"/>&#160;
	<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/>
	</td>
</tr>
</table>
</form>
</xsl:template>

</xsl:stylesheet>