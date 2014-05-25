<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="html" indent="yes"/>
    <xsl:template match="YearlyWorks">
        <html>
            <head>
                <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
                <title>Белорусский национальный технический университет</title>
                <style>
                    <![CDATA[
@font-face
{
	font-family:Impact;
	panose-1:2 11 8 6 3 9 2 5 2 4;
}
p.MsoNormal, li.MsoNormal, div.MsoNormal
{
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:justify;
	line-height:150%;
	font-size:12.0pt;
	font-family:"Times New Roman";
}
@page Section1
{
	size:595.3pt 841.9pt;
	margin:1.0cm 42.55pt 2.0cm 70.9pt;
}
div.Section1
{
	page:Section1;
}
]]>
                </style>
            </head>
            <body lang="RU">
                <div class="Section1">
                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="638" valign="bottom" style='width:478.5pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <xsl:value-of select='string(@Univer)' disable-output-escaping='no'/>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td width="638" valign="bottom" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0" width="758"
                     style='width:568.5pt;border-collapse:collapse'>
                        <tr>
                            <td width="79" valign="top" style='width:59.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:10.0pt'>Факультет</span>
                                </p>
                            </td>
                            <td width="402" valign="top" style='width:302.3pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 1.4pt 0cm 1.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <xsl:value-of select='string(@Faculty)' disable-output-escaping='no'/>
                                </p>
                            </td>
                            <td width="277" valign="top" style='width:208.3pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>            </p>
                            </td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="271" colspan="2" valign="top" style='width:403.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="right" style='text-align:right;line-height:normal'>
                                    <b>«УТВЕРЖДАЮ»</b>
                                </p>
                            </td>
                            <!--<td width="367" valign="top" style='width:275.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
              </td>-->
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="151" colspan="3" valign="top" style='width:104.0cm;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                                    <span style='font-size:10.0pt'>Заведующий кафедрой</span>
                                </p>
                            </td>
                            <!--<td width="168" valign="top" style='width:126.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'><![CDATA[ ]]></p>
              </td>-->
              <td width="319" valign="top" style='width:239.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
                <p class="MsoNormal" style='line-height:normal'>            </p>
              </td>
                        </tr>
                        <tr>
                            <td width="151" colspan="2" valign="top" style='width:114.0cm;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="right" style='text-align:right;line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            
                            <td width="319" valign="top" style='width:209.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                          </td>
                        <td width="168" valign="top" style='width:126.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                            <p class="MsoNormal" style='line-height:normal'>
                                <span style='font-size:10.0pt;text-align:right;'>Н.Н.Гурский</span>
                            </p>
                        </td>
                        </tr>
                        <tr>
                            <td width="127" valign="top" style='width:105.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="192" colspan="2" valign="top" style='width:144.0pt;border:none;border-top:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="right" style='text-align:right;line-height:normal'>
                                    <span style='font-size:8.0pt'>(подпись)</span>
                                </p>
                            </td>
                            <td width="319" valign="top" style='width:239.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <tr height="0">
                            <td width="127" style='border:none'></td>
                            <td width="24" style='border:none'></td>
                            <td width="168" style='border:none'></td>
                            <td width="319" style='border:none'></td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="22" valign="top" style='width:1188.8pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                                    <span style='font-size:10.0pt'>«</span>
                                </p>
                            </td>
                            <td width="33" valign="top" style='width:24.6pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="24" valign="top" style='width:18.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                                    <span style='font-size:10.0pt'>»</span>
                                </p>
                            </td>
                            <td width="132" valign="top" style='width:344.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="427" valign="top" style='width:20.1pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal; text-align:right;'>
                                    <span style='font-size:10.0pt'>
                                        <xsl:value-of select='string(@year)' disable-output-escaping='no'/>
                                    </span>
                                </p>
                            </td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="1" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse;border:none'>
                        <tr>
                            <td width="638" valign="top" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal; height:50pt'>
                                    <![CDATA[ ]]> 
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td width="638" valign="top" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <span style='font-size:11.0pt;font-family:Impact;letter-spacing:3.0pt'>ЗАДАНИЕ ПО ДИПЛОМНОМУ ПРОЕКТИРОВАНИЮ</span>
                                </p>
                            </td>
                        </tr>
                        <tr style='height:12.75pt'>
                            <td width="638" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:32.75pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <![CDATA[ ]]>
                                </p>
                            </td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="79" valign="top" style='width:59.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="72" valign="bottom" style='width:144.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:11.0pt'>студенту-дипломнику</span>
                                </p>
                            </td>
                            <td width="60" valign="bottom" style='width:45.1pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:11.0pt'>группы</span>
                                </p>
                            </td>
                            <td width="114" valign="bottom" style='width:25.8pt;border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Group"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            <td width="79" valign="top" style='width:59.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="312" valign="bottom" style='width:234.2pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Student"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            
                           
                        </tr>
                        <tr>
                            <td width="79" valign="top" style='width:59.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="72" valign="bottom" style='width:54.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="60" valign="bottom" style='width:45.1pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="114" valign="bottom" style='width:25.8pt;border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:8.0pt'>номер</span>
                                </p>
                            </td>
                            <td width="79" valign="top" style='width:59.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="312" valign="bottom" style='width:234.2pt;border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:8.0pt'>инициалы и фамилия</span>
                                </p>
                            </td>
                            
                           
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                    style='border-collapse:collapse'>
                        <tr>
                            
                            <td width="72" valign="bottom" style='width:74.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:11.0pt'>Специальность</span>
                                </p>
                            </td>
                            
                            <td width="114" valign="bottom" style='width:95.8pt;border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="SpecialtyShifr"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            
                            <td width="312" valign="bottom" style='width:534.2pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Specialty"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>


                        </tr>
                        <tr>
                            
                            <td width="79" valign="top" style='width:74.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:11.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            
                            <td width="60" valign="bottom" style='width:95.1pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:8.0pt'>шифр</span>
                                </p>
                            </td>
                            
                            <td width="79" valign="top" style='width:534.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:8.0pt'>наименование специальности</span>
                                </p>
                            </td>
                           
                        </tr>
                    </table>
                    <br/>
                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                    style='border-collapse:collapse'>
                        <tr>
                           
                            <td width="72" valign="bottom" style='width:74.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:11.0pt'>Специализация</span>
                                </p>
                            </td>
                            
                            <td width="114" valign="bottom" style='width:95.8pt;border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="SpecializationShifr"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            
                            <td width="312" valign="bottom" style='width:534.2pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Specialization"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>


                        </tr>
                        <tr>
                            
                            <td width="79" valign="top" style='width:74.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            
                            <td width="60" valign="bottom" style='width:85.1pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:8.0pt'>шифр</span>
                                </p>
                            </td>
                            
                            <td width="79" valign="top" style='width:534.4pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:8.0pt'>наименование специализации</span>
                                </p>
                            </td>

                        </tr>
                    </table>
                    
                    
                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr style='height:17.55pt'>
                            <td width="115" valign="bottom" style='width:86.4pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>1. Тема проекта</span>
                                </p>
                            </td>
                            <td width="523" colspan="7" valign="bottom" style='width:392.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Theme" and number(@line)=0])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="Theme" and number(@line)>0]' />
                        <tr style='height:17.55pt'>
                            <td width="295" colspan="5" valign="bottom" style='width:221.4pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>2. Срок сдачи студентом законченного проекта</span>
                                </p>
                            </td>
                            <td width="343" colspan="3" valign="bottom" style='width:257.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Deadline"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <tr style='height:17.55pt'>
                            <td width="199" colspan="3" valign="bottom" style='width:149.4pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>3. Исходные данные к проекту</span>
                                </p>
                            </td>
                            <td width="439" colspan="5" valign="bottom" style='width:329.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="InputData" and number(@line)=0])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="InputData" and number(@line)>0]' />
                        <tr style='height:17.55pt'>
                            <td width="307" colspan="6" valign="bottom" style='width:230.4pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>4. Содержание расчетно-пояснительной записки</span>
                                </p>
                            </td>
                            <td width="331" colspan="2" valign="top" style='width:248.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="RPZContent" and number(@line)=0])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="RPZContent" and number(@line)>0]' />
                        <tr style='height:17.55pt'>
                            <td width="235" colspan="4" valign="bottom" style='width:176.4pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>5. Перечень графического материала</span>
                                </p>
                            </td>
                            <td width="403" colspan="4" valign="bottom" style='width:302.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="DrawMaterials" and number(@line)=0])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="DrawMaterials" and number(@line)>0]' />
                        <tr style='height:17.55pt'>
                            <td width="367" colspan="7" valign="bottom" style='width:275.4pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>6. Консультанты по проекту (с указанием разделов проекта)</span>
                                </p>
                            </td>
                            <td width="271" valign="bottom" style='width:203.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Consultants" and number(@line)=0])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="Consultants" and number(@line)>0]' />
                        <tr style='height:17.55pt'>
                            <td width="151" colspan="2" valign="bottom" style='width:4.0cm;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>7. Дата выдачи задания</span>
                                </p>
                            </td>
                            <td width="487" colspan="6" valign="bottom" style='width:365.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="PublishData"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                        </tr>
                        <tr style='height:17.55pt'>
                            <td width="367" colspan="7" valign="bottom" style='width:275.4pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>8.</span>
                                    <span style='font-size:10.0pt'>Календарный график работы над проектом на весь период</span>
                                </p>
                            </td>
                            <td width="271" valign="top" style='width:203.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <tr style='height:17.55pt'>
                            <td width="295" colspan="5" valign="bottom" style='width:221.4pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>(с указанием трудоемкости отдельных этапов)</span>
                                </p>
                            </td>
                            <td width="343" colspan="3" valign="bottom" style='width:257.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="Workflow" and number(@line)>=0]' />
                        <tr height="0">
                            <td width="115" style='border:none'></td>
                            <td width="36" style='border:none'></td>
                            <td width="48" style='border:none'></td>
                            <td width="36" style='border:none'></td>
                            <td width="60" style='border:none'></td>
                            <td width="12" style='border:none'></td>
                            <td width="60" style='border:none'></td>
                            <td width="271" style='border:none'></td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                          
                            <td width="156" valign="top" style='width:140.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal; text-align:left;'>
                                    <b>Руководитель</b>
                                </p>
                            </td>
                            <td width="204" valign="top" style='width:83.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="72" valign="top" style='width:54.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="204" valign="top" style='width:153.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <tr>
                            
                            <td width="156" valign="top" style='width:140.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <b><![CDATA[ ]]></b>
                                </p>
                            </td>
                            <td width="204" valign="top" style='width:83.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>подпись, дата</span>
                                </p>
                            </td>
                            <td width="72" valign="top" style='width:54.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="72" valign="top" style='width:153.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:8.0pt'>фамилия, инициалы</span>
                                </p>
                            </td>
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'>
                        <span lang="EN-US"><![CDATA[ ]]></span>
                    </p>

                    <table class="MsoTableGrid" border="1" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse;border:none'>
                        <tr style='height:17.55pt'>
                            <td width="199" valign="bottom" style='width:140.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>Студент-дипломник</span>
                                </p>
                            </td>
                            <td width="204" valign="top" style='width:83.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="72" valign="top" style='width:54.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="204" valign="top" style='width:153.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                       
                        <tr style='height:17.55pt'>
                            <td width="199" valign="bottom" style='width:140.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'>Задание принял к исполнению</span>
                                </p>
                            </td>
                            <td width="204" valign="top" style='width:83.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="72" valign="top" style='width:54.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="204" valign="top" style='width:153.0pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <tr>

                            <td width="156" valign="top" style='width:140.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <b><![CDATA[ ]]></b>
                                </p>
                            </td>
                            <td width="204" valign="top" style='width:83.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>подпись, дата</span>
                                </p>
                            </td>
                            <td width="72" valign="top" style='width:54.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                            </td>
                            <td width="72" valign="top" style='width:153.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:8.0pt'>фамилия, инициалы</span>
                                </p>
                            </td>
                        </tr>
                       
                    </table>

                    <p class="MsoNormal" style='line-height:normal'>
                        <span lang="EN-US"><![CDATA[ ]]></span>
                    </p>

                </div>

            </body>

        </html>
    </xsl:template>

    <xsl:template match="item">
        <tr style='height:17.55pt'>
            <td width="638" colspan="8" valign="bottom" style='width:478.5pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                    <i>
                        <xsl:value-of select='string(.)' disable-output-escaping='no'/>
                    </i>
                </p>
            </td>
        </tr>
    </xsl:template>
</xsl:stylesheet>