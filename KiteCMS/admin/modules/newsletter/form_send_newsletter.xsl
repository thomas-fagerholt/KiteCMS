<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="newsitemid" select="-1"/>
<xsl:param name="categoryid" select="-1"/>

<xsl:template match="/">

<script type="text/javascript">
	var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
	<xsl:if test="$newsitemid!=-1">
		var newsitem=true;
	</xsl:if>
	<xsl:if test="$newsitemid=-1">
		var newsitem=false;
	</xsl:if>
function funcsubmit(){
	if(document.tempForm.subject.value==''){
		alert('<xsl:value-of select="util:localText('subjectnotempty')"/>');
		document.tempForm.subject.focus();
		return false;
	}
	if (newsitem) {
		if(document.getElementById('indhold').value==''){
			alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
			return false;
		}
	} else {
		if(isIE) {
			if(document.getElementById('tbContent_indhold').innerHTML=='') {
				alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
  			return false;
  		}
  	} else {
  	 if (document.getElementById('tbContent_indhold').contentWindow.document.body.innerHTML =='') {
				alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
  			return false;
  		}
	  }
  }

  document.tempForm.action.value='dosend';
  document.tempForm.target='_self';

  if (newsitem) {
    document.tempForm.btnSend.disabled = true;
    document.tempForm.submit();
    return true;
  } else {
    MENU_FILE_SAVE_onclick();
    return false;
  }
  }
  function functest(){
  var emailReg = '^[\\w-_\.]*[\\w-_\.]\@[\\w]\.+[\\w]+[\\w]$';
  var regex = new RegExp(emailReg);
  if(document.tempForm.subject.value==''){
  alert('<xsl:value-of select="util:localText('subjectnotempty')"/>');
		document.tempForm.subject.focus();
		return false;
	}
	if (newsitem) {
		if(document.getElementById('indhold').value==''){
			alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
			return false;
		}
	} else {
		if(isIE) {
			if(document.getElementById('tbContent_indhold').innerHTML=='') {
				alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
  			return false;
  		}
  	} else {
  	 if (document.getElementById('tbContent_indhold').contentWindow.document.body.innerHTML =='') {
				alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
  			return false;
  		}
	  }
	}

	document.tempForm.target='newwindow';
	document.tempForm.action.value='testsend';
	document.tempForm.email.value=prompt('<xsl:value-of select="util:localText('emailtestsending')"/>');
  if(!regex.test(document.tempForm.email.value)){
  	alert('E-mail adressen til testsending er ikke gyldig. Prøv igen med en gyldig email adresse');
  } else {
		window.open('about:blank','newwindow','width=800,height=250,resizable=0,scrollbars=no,menubar=no' ); 

		if (newsitem) {
			document.tempForm.submit();
			return true;
		} else {
		  MENU_FILE_SAVE_onclick();
			return false;
		}
	}
}
</script>

<xsl:text disable-output-escaping="yes">
<![CDATA[
  <form action="default.aspx" target="_self" method="post" name="tempForm" onsubmit="return funcsubmit();">
]]>
</xsl:text>	

<input type="hidden" name="itemid" value="{$newsitemid}"/>
<input type="hidden" name="categoryid" value="{$categoryid}"/>
<input type="hidden" name="email" value=""/>

<xsl:for-each select="//emailcategory[((newsitem/@id=$newsitemid and $newsitemid!=-1)or(@id=$categoryid and $categoryid!=-1)) and @pageid=//selectedpage]">
	<table border="0" style="margin-left:150px;">
	<tr>
		<td valign="top" align="right"><b><xsl:value-of select="util:localText('categorytitle')"/></b>:</td>
		<td><b><xsl:value-of select="title"/></b></td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('subject')"/>:</td>
		<td><input type="text" name="subject" class="alminput" size="50" value="{newsitem[@id=$newsitemid]/title}" maxlength="250"/></td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('header')"/>:</td>
		<td><xsl:value-of select="header"/></td>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('content')"/>:</td>
		<xsl:if test="$newsitemid!=-1">
			<td><textarea name="indhold" id="indhold" cols="100" rows="20" class="alminput" style="border:dotted 1px black;" disabled="true"><xsl:value-of select="newsitem[@id=$newsitemid]/content"/></textarea></td>
		</xsl:if>
		<xsl:if test="$newsitemid=-1">
			<td>
				<Editor id="indhold" configfile="/admin/data/editornosave.xml" NoFormtag="true" hidden="false" height="300px" width="500px" />
			</td>
		</xsl:if>
	</tr>
	<tr>
		<td valign="top" align="right"><xsl:value-of select="util:localText('footer')"/>:</td>
		<td><xsl:value-of select="footer"/></td>
	</tr>
	<tr>
		<td></td>
		<td>
		<xsl:element name="input">
      <xsl:attribute name="type">button</xsl:attribute>
      <xsl:attribute name="name">btnSend</xsl:attribute>
      <xsl:attribute name="value"><xsl:value-of select="util:localText('send')"/></xsl:attribute>
		<xsl:attribute name="class">almknap</xsl:attribute>
		<xsl:attribute name="onclick">if(confirm('<xsl:value-of select="util:localText('oktosend')"/>')) {return funcsubmit();return false;}</xsl:attribute>
		</xsl:element>&#160;
		<input type="button" class="almknap" value="{util:localText('test')}" onclick="return functest();return false;"/>&#160;
		<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
		</td>
	</tr>
	</table>

	<xsl:if test="$newsitemid!=-1">
		<input type="hidden" name="pageid" value="{//selectedpage}"/>
		<input type="hidden" name="action" value=""/>
		<xsl:text disable-output-escaping="yes">
		<![CDATA[
		  </form>
		]]>
		</xsl:text>	
	</xsl:if>
</xsl:for-each>
</xsl:template>

</xsl:stylesheet>