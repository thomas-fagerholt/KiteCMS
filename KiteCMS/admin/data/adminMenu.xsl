<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="message" select="-1" />
  <xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<xsl:template match="/">

<script type="text/javascript">
	function postInit() {
  <xsl:for-each select="//page[@id=$selectedpage]/ancestor-or-self::page">
    adminFoldMenuShow('parentid<xsl:value-of select="@id"/>');
  </xsl:for-each>
	}
</script>
<xsl:text disable-output-escaping="yes">
<![CDATA[
  <script type="text/javascript">

  var onMenu;
  var timeOn;
  
  function showmenu(img, menuid){
  if (menuid != '') {
  document.getElementById(menuid).style.top = img.offsetHeight + 10 + 'px';
  document.getElementById(menuid).style.left = img.offsetLeft + 'px';
  document.getElementById(menuid).style.display='block';
	 var objall = document.getElementsByTagName('object');
	 for (var i=0;i!=objall.length;i++)
		objall[i].style.visibility = 'hidden';
  }
  if (timeOn != null) {
  clearTimeout(timeOn)
  if (onMenu != menuid)
  if(document.getElementById(onMenu))
  document.getElementById(onMenu).style.display='none';
  }
  if (menuid != '')
    onMenu = menuid;
  }

  function mouseover() {
    if (timeOn != null)
    clearTimeout(timeOn);
  }

  function mouseout(menuid) {
  if (menuid!='')
  timeOn = setTimeout("hidemenu('"+ menuid +"');", 750);
  }

  function hidemenu(oMenu) {
  document.getElementById(oMenu).style.display='none';
  document.getElementById('KiteCMSAdminBarInner').style.display='none';
  document.getElementById('KiteCMSAdminBarShortcuts').style.display='none';
  document.getElementById('KiteCMSAdminMessage').innerHTML='';
	 var objall = document.getElementsByTagName('object');
	 for (var i=0;i!=objall.length;i++)
		objall[i].style.visibility = 'visible';
  }

  // Collapse menu code
  var adminFoldMenuNode = [];

  function adminFoldMenuSet(m, c) {
   if (document.getElementById && document.createElement) {
	  m = document.getElementById(m).getElementsByTagName('ul');
	  var d, p, x, h, i, j;
	  for (i = 0; i < m.length; i++) {
     if (d = m[i].getAttribute('id')) {
  		adminFoldMenuCtrl(d, c, 'x', '<img src="/admin/images/plus.gif" border="0" alt="open"/>', 'Show');
			x = adminFoldMenuCtrl(d, c, 'c', '<img src="/admin/images/minus.gif" border="0" alt="close"/>', 'Hide');

 		  p = m[i].parentNode;
		  if (h = !p.className) {
			 j = 2;
			 while ((h = !(d == arguments[j])) && (j++ < arguments.length));
			 if (h) {
			  m[i].style.display = 'none';
			  x = adminFoldMenuNode[d+'x'];
			 }
		  }

		  p.className = c;
		  p.insertBefore(x, p.firstChild);
	   }
	  }
   }
  // unfold currentNode
  postInit();
  }

function adminFoldMenuShow(m) {
	adminFoldMenuXC(m, 'block', m+'c', m+'x');
}


function adminFoldMenuHide(m) {
	adminFoldMenuXC(m, 'none', m+'x', m+'c');
}


function adminFoldMenuXC(e, d, s, h) {
	if (document.getElementById(e)) {
 	 e = document.getElementById(e);
	 e.style.display = d;
	 e.parentNode.replaceChild(adminFoldMenuNode[s], adminFoldMenuNode[h]);
   adminFoldMenuNode[s].firstChild.style.display = 'inline';
   adminFoldMenuNode[s].firstChild.className = 'adminFoldMenuInline';
  }
}


function adminFoldMenuCtrl(m, c, s, v, f) {
  var a = document.createElement('a');
	a.setAttribute('href', 'javascript:adminFoldMenu'+f+'(\''+m+'\');');
	a.innerHTML = v;

	var d = document.createElement('div');
	d.className = c+s;
	d.appendChild(a);

	return adminFoldMenuNode[m+s] = d;
}
var nowOnload = window.onload;
window.onload = function() {adminFoldMenuSet('dropmenu', 'adminFoldMenu', 'js');if(nowOnload != null && typeof(nowOnload) == 'function') { nowOnload();} };
</script>
]]>
</xsl:text>

<xsl:variable name="displayvalue">
    <xsl:if test="$message='-1'">none</xsl:if>
    <xsl:if test="not($message='-1')">block</xsl:if>
  </xsl:variable>

  <xsl:variable name="modulemenu"><xsl:value-of select="util:GetModulPageMenu($selectedpage)"/></xsl:variable>

  <div id="KiteCMSAdminBar" onclick="document.getElementById('KiteCMSAdminBarInner').style.display='block';document.getElementById('KiteCMSAdminBarShortcuts').style.display='block'" onmouseover="document.getElementById('KiteCMSAdminBarInner').style.display='block';document.getElementById('KiteCMSAdminBarShortcuts').style.display='block'">
	  <div id="KiteCMSAdminBarInner" style="display:{$displayvalue};">
      <xsl:for-each select="//page[@id=$selectedpage]">
        <xsl:variable name="thismoduleid">
          <xsl:value-of select="usetemplate"/>
        </xsl:variable>
        <xsl:if test="util:userHasAccess(1201,$selectedpage)">
          <a href="{//template[@id=$thismoduleid]/publicurl}?pageid={$selectedpage}&amp;action=editcontent"><img src="/admin/images/edit_page.gif" width="23" height="25" alt="{util:localText('editcontent')}" title="{util:localText('editcontent')}" style="border:none" onmouseover="showmenu(this,'');" onmouseout="mouseout('KiteCMSAdminBarInner');" class="firstimage"/></a>
        </xsl:if>  
        <xsl:if test="not(util:userHasAccess(1201,$selectedpage))">
          <img src="/admin/images/space.gif" width="38" height="24" alt="" style="border:none"/>
        </xsl:if>  
      </xsl:for-each>

      <img src="/admin/images/page_properties.gif" width="23" height="24" alt="{util:localText('pageproperties')}" title="{util:localText('pageproperties')}" onclick="showmenu(this,'KiteCMSAdminPageprop');" onmouseover="showmenu(this,'KiteCMSAdminPageprop');" onmouseout="mouseout('KiteCMSAdminPageprop');"/>
      <img src="/admin/images/site_functions.gif" width="23" height="24" alt="{util:localText('siteproperties')}" title="{util:localText('siteproperties')}" onclick="showmenu(this,'KiteCMSAdminSiteprop');" onmouseover="showmenu(this,'KiteCMSAdminSiteprop');" onmouseout="mouseout('KiteCMSAdminSiteprop');"/>
      <img src="/admin/images/menu.gif" width="23" height="24" alt="{util:localText('choosemenuitem')}" title="{util:localText('choosemenuitem')}" onclick="showmenu(this,'KiteCMSAdminMenu');" onmouseover="showmenu(this,'KiteCMSAdminMenu');" onmouseout="mouseout('KiteCMSAdminMenu');"/>

      <xsl:if test="$modulemenu!=''">
        <img src="/admin/images/page_module.gif" width="23" height="24" alt="{util:localText('moduleproperties')}" title="{util:localText('moduleproperties')}" onclick="showmenu(this,'KiteCMSAdminPagemodule');" onmouseover="showmenu(this,'KiteCMSAdminPagemodule');" onmouseout="mouseout('KiteCMSAdminPagemodule');"/>
        <div id="KiteCMSAdminPagemodule" class="adminSubMenu" style="display:none;top:0;left:0" onmouseout="mouseout(this.id);" onmouseover="mouseover();">
          <xsl:value-of select="$modulemenu" disable-output-escaping="yes"/>
        </div>
      </xsl:if>

      <span id="KiteCMSAdminMessage">
        <xsl:if test="not($message='-1')">
          <xsl:value-of select="$message"/>
        </xsl:if>
      </span>
  </div>
    <div id="KiteCMSAdminBarShortcuts">
      <ul>
        <xsl:value-of select="util:GetAdminShortcuts($selectedpage)" disable-output-escaping="yes"/>
        <li>
          <a href="#" onclick="window.open('http://www.KiteCMS.com/help','_blank','width=700,height=500,toolbar=no,status=no,scrollbars=yes')">
            <img src="/admin/images/help.gif" alt="{util:localText('help')}" title="{util:localText('help')}" width="13" height="13"/>
          </a>
        </li>
        <li>
          <a href="/admin/access/logout.aspx">
            <img src="/admin/images/logoff.gif" alt="{util:localText('logoff')}" title="{util:localText('logoff')}" width="15" height="14"/>
          </a>
        </li>
      </ul>
    </div>
  </div>

  <xsl:variable name="scrollClass">
    <xsl:if test="count(//page)>35">scrollbar</xsl:if>
  </xsl:variable>
  <div id="KiteCMSAdminMenu" class="adminSubMenu {$scrollClass}" style="display:none;top:0;left:0" onmouseout="mouseout(this.id);" onmouseover="mouseover();">
    <h2>
      <xsl:value-of select="util:localText('choosemenuitem')"/>
    </h2>
    <ul id="dropmenu">
      <xsl:apply-templates select="/website/page"/>
    </ul>
  </div>
</xsl:template>
  
<xsl:template match="page">
	<xsl:for-each select=".">	
		<xsl:variable name="moduleid">
		<xsl:value-of select="usetemplate"/>
		</xsl:variable>

		<xsl:variable name="menutitle">
			<xsl:choose>
				<xsl:when test="string-length(menutitle)>15">
					<xsl:value-of select="substring(menutitle,0,13)"/>...
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="menutitle"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

    <li>
	
			<xsl:element name="a">
			<xsl:attribute name="title"><xsl:value-of select="menutitle"/></xsl:attribute>
			<xsl:attribute name="href"><xsl:value-of select="//template[@id=$moduleid]/publicurl"/>?pageid=<xsl:value-of select="@id"/></xsl:attribute>

        <xsl:if test="public='-1'">(</xsl:if>
        <xsl:if test="public='0'">{</xsl:if>
        <xsl:if test="not(//template[@id=$moduleid]/templatecolor='')">
				<span style="color:{//template[@id=$moduleid]/templatecolor}">
				<xsl:if test="not(@id=$selectedpage)">
					<xsl:value-of select="$menutitle"/>
				</xsl:if>
				<xsl:if test="@id=$selectedpage">
					<b>.:<xsl:value-of select="$menutitle"/>:.</b>
				</xsl:if>
				</span>
			</xsl:if>
			<xsl:if test="//template[@id=$moduleid]/templatecolor=''">
				<xsl:if test="not(@id=$selectedpage)">
					<xsl:value-of select="$menutitle"/>
				</xsl:if>
				<xsl:if test="@id=$selectedpage">
					<b>.:<xsl:value-of select="$menutitle"/>:.</b>
				</xsl:if>
			</xsl:if>
        <xsl:if test="public='-1'">)</xsl:if>
        <xsl:if test="public='0'">}</xsl:if>
      </xsl:element>
	
      <!-- subpages ?-->
      <xsl:if test="count(./page)>0">
        <ul id="parentid{@id}">
          <xsl:apply-templates select="page"/>
        </ul>
      </xsl:if>
    </li>
  </xsl:for-each>

</xsl:template>

</xsl:stylesheet>

