<?xml version="1.0" encoding="ISO-8859-1" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" xmlns:access="urn:userHasAccess" version="1.0" exclude-result-prefixes="util access">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="hasRoot" select="-1" />

<xsl:template match="/">
		<xsl:variable name="fewFirstLevel"><xsl:choose><xsl:when test="count(/website/page)&lt;=3">2</xsl:when><xsl:otherwise>1</xsl:otherwise></xsl:choose></xsl:variable>

		<h4><xsl:value-of select="util:localText('choosenewlocation')"/> '<xsl:value-of select="//page[@id=//selectedpage]/menutitle"/>':</h4>
		<p class="KiteCMSDefaultText chooseLink">

		<xsl:for-each select="//page">	
			<xsl:variable name="prev">
				<xsl:value-of select="preceding-sibling::page[position()=1]/@id"/>
			</xsl:variable>

			<xsl:choose>
				<xsl:when test="count(ancestor-or-self::page[@id=//selectedpage])=0 and count(//page[@id=$prev]/ancestor-or-self::page[@id=//selectedpage])=0">
					<xsl:if test="access:userHasAccess(1102,@id,0,1)">
						<xsl:for-each select="ancestor::page">
							<img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
						</xsl:for-each>
			 			<img align="absmiddle" src="/admin/images/dottedCross.gif" width="20px" height="18px"/>
						<a href="movemenu.aspx?pageid={//selectedpage}&amp;action=move&amp;child={@id}">
							<img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" title="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
						</a>
					</xsl:if>
				</xsl:when>
				<xsl:otherwise>
					<img align="absmiddle" src="/admin/images/space.gif" width="11px" height="1px" alt=""/>
				</xsl:otherwise>
			</xsl:choose>

			<xsl:if test="count(ancestor::page)&lt;$fewFirstLevel">
				&#160;<img align="absmiddle" src="/admin/images/space.gif" width="500px" height="1px" class="dottedSpace"/>
			</xsl:if>
			<br/>

			<xsl:for-each select="ancestor::page">
				<img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
			</xsl:for-each>
			 <img align="absmiddle" src="/admin/images/dottedCross.gif" width="20px" height="18px"/>

				<xsl:if test="count(ancestor-or-self::page[@id=//selectedpage])>0">
					<b><xsl:value-of select="menutitle"/></b>
				</xsl:if>
				<xsl:if test="count(ancestor-or-self::page[@id=//selectedpage])=0">
					<xsl:value-of select="menutitle"/>
				</xsl:if>
				
				<xsl:if test="count(child::page)=0 and count(ancestor-or-self::page[@id=//selectedpage])=0">
					<xsl:if test="access:userHasAccess(1102,@id,0,1)">
						<xsl:element name="a">
							<xsl:attribute name="href">movemenu.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=move&amp;parent=<xsl:value-of select="@id"/></xsl:attribute>
							<xsl:attribute name="title">nyt underpunkt</xsl:attribute>
							 <img align="absmiddle" src="/admin/images/space.gif" width="4px" height="4px"/>
							 <img align="absmiddle" src="/admin/images/newMenu2.gif" alt="{util:localText('add')} {util:localText('subitem')}" title="{util:localText('add')} {util:localText('subitem')}" width="11" height="18px"/>
						</xsl:element>
					</xsl:if>
				</xsl:if>
				
			<!-- indsaet som sidste paa et niveau -->
			<xsl:for-each select="ancestor::page">
			<xsl:sort select="position()" order="descending"/>
				<xsl:if test="count(following-sibling::page)=0 and count(child::page)=0 and count(ancestor-or-self::page[@id=//selectedpage])=0">
				<xsl:if test="access:userHasAccess(1102,@id,0,1)">
					<br/>
					<xsl:for-each select="ancestor::page">
						<img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
					</xsl:for-each>
					<xsl:element name="a">
					<xsl:attribute name="href">movemenu.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=move&amp;parent=<xsl:value-of select="../@id"/></xsl:attribute>
					<img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" title="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
					</xsl:element>
				</xsl:if>
			</xsl:if>
		</xsl:for-each>

			<br/>
			<!-- insert page as last in menu without children -->
			<xsl:if test="count(ancestor-or-self::page[@id=//selectedpage])=0">
				<xsl:if test="count(following-sibling::page)=0 and position()!=last()">
					<xsl:if test="count(child::page)=0">
						<xsl:for-each select="ancestor::page">
							<img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
						</xsl:for-each>
						<img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
						<xsl:if test="access:userHasAccess(1102,@id,0,1)">
							<a href="movemenu.aspx?pageid={//selectedpage}&amp;action=move&amp;child=-1&amp;parent={../@id[.!=0]}">
								<img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" title="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
							</a>
						</xsl:if>
						<br/>
					</xsl:if>
				</xsl:if>
			</xsl:if>

			<xsl:variable name="lastNodeId"><xsl:value-of select="@id"/></xsl:variable>

			<!-- insert page as last in menu with children -->
			<xsl:if test="count(following-sibling::page)=0 and position() &lt; last() and count(child::page)=0">
		
				<xsl:for-each select="ancestor::page">
				<xsl:sort select="position()" order="descending"/>
					<xsl:if test="count(ancestor-or-self::page[@id=//selectedpage])=0">
		
						<xsl:variable name="match">  
							<xsl:for-each select="current()/descendant::page">    
								<xsl:if test="//page[@id=$lastNodeId]/following::page[@id=current()/@id]">      
									<xsl:copy-of select="."/>    
								</xsl:if>  
							</xsl:for-each>
						</xsl:variable>
		
						<xsl:if test="$match='' and count(following-sibling::page)=0">
							<xsl:for-each select="ancestor::page">
								<img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
							</xsl:for-each>
								<xsl:if test="count(ancestor::page)&gt;=$fewFirstLevel">
									<img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
								</xsl:if>
								<xsl:if test="access:userHasAccess(1102,@id,0,1) and position() != last()">
								<a href="movemenu.aspx?pageid={//selectedpage}&amp;action=move&amp;child=-1&amp;parent={../@id[.!=0]}">
									<img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" title="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
								</a>
								<br/>
							</xsl:if>
						</xsl:if>
					</xsl:if>
				</xsl:for-each>
			</xsl:if>

	</xsl:for-each>

		</p>

		<br/>
		<form action="/default.aspx">
			<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</form>
	
</xsl:template>

</xsl:stylesheet>

