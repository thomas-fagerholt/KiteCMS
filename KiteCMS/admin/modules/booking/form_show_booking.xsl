<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="week" select="-1" />
<xsl:param name="month" select="-1" />
<xsl:param name="year" select="-1" />
<xsl:param name="document" select="default.aspx" />
<xsl:param name="blnCanAdd" select="-1" />
<xsl:param name="blnCanEdit" select="-1" />

<xsl:template match="/">
<xsl:variable name="selecteditem"><xsl:value-of select="//selecteditem"/></xsl:variable>
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<!-- vælg ressource -->

<form method="get" name="booking" action="{$document}">
<input type="hidden" name="pageid" value="{$selectedpage}"/>
<input type="hidden" name="action" value="show"/>
<select name="item" onChange="document.booking.submit();">
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
&#160;

<xsl:value-of select="util:localText('jumpweek')"/>:
<select name="week" onChange="document.booking.submit();">
	<option value="{$week}Y{$year}"><xsl:value-of select="util:localText('chooseweek')"/></option>
	<xsl:for-each select="/website/week">
		<option value="{@value}"><xsl:value-of select="."/></option>
	</xsl:for-each>
</select>
</form>

<!-- kalender visning -->
<table cellpadding="1" cellspacing="1" border="0">
<tr>
<td align="center" width="100" height="40" class="bookingHeader">
<nobr>
<a href="{$document}?pageid={$selectedpage}&amp;action=show&amp;item={$selecteditem}&amp;move=-1&amp;week={$week}&amp;year={$year}"><img src="/modules/booking/images/trileft.gif" border="0" alt="gå en uge tilbage"/></a>
&#160;<xsl:value-of select="util:localText('week')"/>&#160;<xsl:value-of select="$week"/>
&#160;<a href="{$document}?pageid={$selectedpage}&amp;action=show&amp;item={$selecteditem}&amp;move=1&amp;week={$week}&amp;year={$year}"><img src="/modules/booking/images/tri.gif" border="0" alt="gå en uge frem"/></a>
</nobr>
</td>
<xsl:for-each select="/website/date">
	<td align="center" width="100" height="40" class="bookingHeader">
	<xsl:call-template name="dateUS2DK">
		<xsl:with-param name='date'><xsl:value-of select="."/></xsl:with-param>
	</xsl:call-template>
	<br/><img src="/images/space.gif" width="100" height="1"/>
	</td>
</xsl:for-each>
</tr>
<xsl:for-each select="/website/time">
	<xsl:variable name="time"><xsl:value-of select="@id"/></xsl:variable>
	<tr>
		<td align="center" valign="top" height="40" class="bookingHeader"><xsl:value-of select="."/></td>
		<!-- mandag -->
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=1)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=1)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[1]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<!-- tirsdag -->
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=2)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=2)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[2]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<!-- onsdag -->
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=3)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=3)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[3]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<!-- torsdag -->
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=4)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=4)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[4]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<!-- fredag -->
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=5)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=5)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[5]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=6)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=6)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[6]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test="count(//item[@id=$selecteditem]/booking/time[(@time=$time)and(@day=7)])>0">
				<td class="bookingTaken" valign="top">
				<xsl:for-each select="//item[@id=$selecteditem]/booking[(time/@time=$time)and(time/@day=7)]">
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[<span class="bookingRepeat">]]></xsl:text></xsl:if>
					<xsl:if test="$blnCanEdit='true'">
						<xsl:element name="a">
							<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=edit&amp;item=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="text"/>&#10;-&#10;klik for at editere</xsl:attribute>
							<xsl:value-of select="header"/>
						</xsl:element>
					</xsl:if>
					<xsl:if test="$blnCanEdit!='true'">
						<div title='{text}'><xsl:value-of select="header"/></div>
					</xsl:if>
					<xsl:if test="@returning!=''"><xsl:text disable-output-escaping="yes"><![CDATA[</span>]]></xsl:text></xsl:if>
					<br/>
				</xsl:for-each>
				</td>
			</xsl:when>
			<xsl:otherwise>
				<td class="bookingFree">
				<xsl:if test="$blnCanAdd='true'">
					<xsl:element name="a">
						<xsl:attribute name="href">default.aspx?pageid=<xsl:value-of select="$selectedpage"/>&amp;action=add&amp;item=<xsl:value-of select="$selecteditem"/>&amp;date=<xsl:call-template name="dateUS2DK"><xsl:with-param name="date"><xsl:value-of select="//website/date[7]"/></xsl:with-param></xsl:call-template>&amp;time=<xsl:value-of select="$time"/></xsl:attribute>
						<img src="/images/space.gif" width="100" height="40" border="0" alt="tilføj booking"/>
					</xsl:element>
				</xsl:if>
				</td>
			</xsl:otherwise>
		</xsl:choose>
	</tr>
</xsl:for-each>
</table>
<br/>
<a href="/modules/default.aspx?pageid={$selectedpage}">[<xsl:value-of select="util:localText('goto')"/>&#160;<xsl:value-of select="util:localText('adminpage')"/>]</a>
</xsl:template>

<xsl:template name="dateUS2DK">
<xsl:param name='date'/>
	<xsl:value-of select="substring-before($date,'/')"/>-<xsl:value-of select="substring-before(substring-after($date,'/'),'/')"/>-<xsl:value-of select="substring-after(substring-after($date,'/'),'/')"/>
</xsl:template>

</xsl:stylesheet>