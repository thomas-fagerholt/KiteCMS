<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

  <xsl:output omit-xml-declaration="yes"/>
  <xsl:template match="/">

    <script type="text/javascript">
      if (document.all)
      window.onload = initIE;
      if (document.addEventListener)
      document.addEventListener("DOMContentLoaded", initFF, false);

      function initIE() {
      document.getElementById('filecontent').innerText = unescape(document.getElementById('filecontent').innerText);
      }

      function initFF() {
      document.getElementById('filecontent').textContent = unescape(document.getElementById('filecontent').textContent );
      }

      function funcSubmit() {
      if (document.getElementById('filecontent').value == '') {
      alert('<xsl:value-of select="util:localText('contentnotempty')"/>');
        return false;
      }
      return true;
      }
    </script>
    
    <form method="post" action="/admin/template/editxslfile.aspx"  id="editform" onsubmit="return funcSubmit();">
  	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	  <input type="hidden" name="action" value="doedit"/>
	  <input type="hidden" name="xslurl" value="{//xslurl}"/>
    <textarea name="filecontent" id="filecontent" cols="90" rows="40"><xsl:value-of select="//filecontent"/></textarea>
    <p>
      <input type="submit" class="almknap" value="{util:localText('save')}"/>
		  &#160;
      <input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-2)"/>
    </p>
    </form>
  </xsl:template>
  </xsl:stylesheet>