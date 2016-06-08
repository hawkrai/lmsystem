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
                  <br></br>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
style='border-collapse:collapse'>
                    <tr>
                      <td width="100%" valign="bottom" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='font-size:15pt;text-align:center;line-height:normal'>
                          <xsl:value-of select='string(item[@name="Univer"])' disable-output-escaping='no'/>
                        </p>
                      </td>
                    </tr>
                    <tr>
                      <td width="100%" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                          <span style='font-size:8.0pt'><![CDATA[ ]]></span>
                        </p>
                      </td>
                    </tr>
                  </table>

                  <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                  <table  class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse;margin-top:10px'>
                    <tr>
                      <td width="100%" valign="top" style='border:none;padding:0cm 1.4pt 0cm 1.4pt'>
                        <p class="MsoNormal" align="center" style='font-size:15pt;text-align:center;line-height:normal'>
                          <xsl:value-of select='string(item[@name="Faculty"])' disable-output-escaping='no'/>
                        </p>
                      </td>
                    </tr>
                  </table>
                  <br></br>
                  <br></br>
                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                  style='border-collapse:collapse'>
                    <tr>
                      <td width="50%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="right" style='text-align:right;line-height:normal'>
                          <span style='font-size:10.0pt'></span>
                        </p>
                      </td>
                      <td width="50%" valign="top">
                        <p class="MsoNormal" align="right" style='line-height:normal'>
                          <span style='font-size:10.0pt'>Утверждаю</span>
                        </p>
                      </td>

                    </tr>
                  </table>

                  <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr>
                      <td width="50%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="135px" valign="top">
                        <p class="MsoNormal" style='line-height:normal;text-align:left;'>
                          <span style='font-size:10.0pt'>Заведующий кафедрой</span>
                        </p>
                      </td>
                      <td width="auto" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="auto" valign="top" style='width:126.0pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:10.0pt;text-align:center;'>
                            <xsl:value-of select='string(item[@name="HeadCathedra"])' disable-output-escaping='no'/>
                          </span>
                        </p>
                      </td>
                    </tr>

                    <tr>
                      <td width="50%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="135px"  valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                        </p>
                      </td>
                      <td width="auto"  valign="top" style='border:none;border-top:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="right" style='text-align:center;line-height:normal'>
                          <span style='font-size:8.0pt'>(подпись)</span>
                        </p>
                      </td>
                      <td width="auto"  valign="top" style='border:none;border-top:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="right" style='line-height:normal'>
                          <span style='font-size:8.0pt'>(фамилия, инициалы)</span>
                        </p>
                      </td>
                    </tr>
                    <tr height="0">
                      <td width="50%" style='border:none'></td>
                      <td width="135px" style='border:none'></td>
                      <td width="auto" style='border:none'></td>
                      <td width="auto" style='border:none'></td>
                    </tr>
                  </table>

                  <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr>
                      <td width="50%" valign="top" >
                        <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                          <span style='font-size:10.0pt'>«</span>
                        </p>
                      </td>
                      <td width="5%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                        </p>
                      </td>
                      <td width="1%" valign="top" >
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:10.0pt'>»</span>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal;'>
                          <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                        </p>
                      </td>
                      <td width="auto" valign="top" >
                        <p class="MsoNormal" style='line-height:normal;'>
                          <span style='font-size:10.0pt'>
                            <xsl:value-of select='string(@year)' disable-output-escaping='no'/>
                          </span>
                        </p>
                      </td>
                      <td width="auto" valign="top">
                        <p class="MsoNormal" style='line-height:normal;text-align:right;'>
                          <span style='font-size:10.0pt'></span>
                        </p>
                      </td>
                    </tr>
                  </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" width="100%"  border="1" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse;border:none'>
                        <tr>
                            <td width="100%" valign="top" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="center" style='text-align:center;line-height:normal; height:50pt'>
                                    <![CDATA[ ]]> 
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top" style='width:478.5pt;border:none;padding:0cm 5.4pt 0cm 5.4pt'>
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

                    <table width="100%" class="MsoTableGrid" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                        <tr>
                            <td width="221px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span>Cтуденту-дипломнику группы</span>
                                </p>
                            </td>
                            <td width="10%" valign="bottom" style='border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Group"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            <td width="2%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="auto" valign="bottom" style='border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Student"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>
                            
                           
                        </tr>
                        <tr>
                            <td width="221px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span style='font-size:10.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="10%" valign="bottom" style='border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>номер</span>
                                </p>
                            </td>
                            <td width="2%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            <td width="auto" valign="bottom" style='border-top:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>инициалы и фамилия</span>
                                </p>
                            </td>
                            
                           
                        </tr>
                    </table>

                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                    <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                    style='border-collapse:collapse'>
                        <tr>
                            
                            <td width="72px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span>Специальность</span>
                                </p>
                            </td>
                            
                            <td width="30%" valign="bottom" style='border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="SpecialtyShifr"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>

                          <td width="2%" valign="bottom" style='border-top:none;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                            <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>

                            </p>
                          </td>
                            
                            <td width="68%" valign="bottom" style='border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Specialty"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>


                        </tr>
                        <tr>
                            
                            <td width="79px" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:11.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            
                            <td width="30%" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>шифр</span>
                                </p>
                            </td>
                          
                          <td width="2%" valign="bottom" style='border-top:none;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                            <p class="MsoNormal" align="left" style='text-align:center;line-height:normal;font-size:11.0pt'>

                            </p>
                          </td>  
                          
                            <td width="68%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>наименование специальности</span>
                                </p>
                            </td>
                           
                        </tr>
                    </table>
                    <br/>
                    <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                    style='border-collapse:collapse'>
                        <tr>
                           
                            <td width="72px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span >Специализация</span>
                                </p>
                            </td>
                            
                            <td width="30%" valign="bottom" style='border-top:none;border-bottom:solid windowtext 1.0pt;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="SpecializationShifr"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>

                          <td width="2%" valign="bottom" style='border-top:none;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                            <p class="MsoNormal" align="left" style='text-align:center;line-height:normal;font-size:11.0pt'>

                            </p>
                          </td>

                          <td width="68%" valign="bottom" style='border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal;font-size:11.0pt'>
                                    <i>
                                        <xsl:value-of select='string(item[@name="Specialization"])' disable-output-escaping='no'/>
                                    </i>
                                </p>
                            </td>


                        </tr>
                        <tr>
                            
                            <td width="79px" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='line-height:normal'>
                                    <span style='font-size:14.0pt'><![CDATA[ ]]></span>
                                </p>
                            </td>
                            
                            <td width="30%" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" align="left" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>шифр</span>
                                </p>
                            </td>

                          <td width="2%" valign="bottom" style='border-top:none;border-right:none;padding:0cm 5.4pt 0cm 5.4pt'>
                            <p class="MsoNormal" align="left" style='text-align:center;line-height:normal;font-size:11.0pt'>

                            </p>
                          </td>

                          <td width="68%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                                <p class="MsoNormal" style='text-align:center;line-height:normal'>
                                    <span style='font-size:8.0pt'>наименование специализации</span>
                                </p>
                            </td>

                        </tr>
                    </table>
                  <br></br>

                  <table class="MsoTableGrid" width ="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="128px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>1. Тема проекта</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="Theme" and number(@line)=0])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                    <xsl:apply-templates select='item[@name="Theme" and number(@line)>0]' />
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="338px"  valign="bottom" style=';padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>2. Срок сдачи студентом законченного проекта</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="EndData"])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="223px"  valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>3. Исходные данные к проекту</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="InputData" and number(@line)=0])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                    <xsl:apply-templates select='item[@name="InputData" and number(@line)>0]' />
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="344px"  valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>4. Содержание расчетно-пояснительной записки</span>
                        </p>
                      </td>
                      <td width="auto"  valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="RPZContent" and number(@line)=0])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                     style='border-collapse:collapse'>
                    <xsl:apply-templates select='item[@name="RPZContent" and number(@line)>0]' />
                    <tr style='height:17.55pt'>
                      <td width="265px"  valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>5. Перечень графического материала</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="DrawMaterials" and number(@line)=0])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                    <xsl:apply-templates select='item[@name="DrawMaterials" and number(@line)>0]' />
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="423px"  valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>6. Консультанты по проекту (с указанием разделов проекта)</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="Consultants" and number(@line)=0])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                  <xsl:apply-templates select='item[@name="Consultants" and number(@line)>0]' />
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr style='height:17.55pt'>
                      <td width="173px" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>7. Дата выдачи задания</span>
                        </p>
                      </td>
                      <td width="auto" valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <i>
                            <xsl:value-of select='string(item[@name="PublishData"])' disable-output-escaping='no'/>
                          </i>
                        </p>
                      </td>
                    </tr>
                  </table>
                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                        <tr style='height:17.55pt'>
                            <td width="430px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span>8. Календарный график работы над проектом на весь период</span>
                                </p>
                            </td>
                            <td width="auto" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <tr style='height:17.55pt'>
                            <td width="430px" valign="bottom" style='padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                                    <span>(с указанием трудоемкости отдельных этапов)</span>
                                </p>
                            </td>
                            <td width="auto"  valign="bottom" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                                <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'><![CDATA[ ]]></p>
                            </td>
                        </tr>
                        <xsl:apply-templates select='item[@name="Workflow" and number(@line)>=0]' />
                    </table>
                  <br></br>
                  <br></br>
                  <br></br>
                    <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>

                  <table class="MsoTableGrid" width="100%" border="0" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse'>
                    <tr>

                      <td width="50%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal; text-align:left;'>
                          <span>Руководитель</span>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="10%" valign="top" style='padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="20%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                    </tr>
                    <tr>

                      <td width="50%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <b><![CDATA[ ]]></b>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                          <span style='font-size:8.0pt'>(подпись, дата)</span>
                        </p>
                      </td>
                      <td width="10%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:8.0pt'>(фамилия, инициалы)</span>
                        </p>
                      </td>
                    </tr>

                  </table>

                  <p class="MsoNormal" style='line-height:normal'>
                    <span lang="EN-US"><![CDATA[ ]]></span>
                  </p>

                  <table class="MsoTableGrid" width="100%" border="1" cellspacing="0" cellpadding="0"
                   style='border-collapse:collapse;border:none'>
                    <tr style='height:17.55pt'>
                      <td width="50%" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span style='font-size:10.0pt'></span>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="10%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                    </tr>

                    <tr style='height:17.55pt'>
                      <td width="50%" valign="bottom" style='border:none;padding:0cm 5.4pt 0cm 5.4pt;height:17.55pt'>
                        <p class="MsoNormal" align="left" style='text-align:left;line-height:normal'>
                          <span>Студент-дипломник задание принял к исполнению</span>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="10%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="20%" valign="top" style='border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                    </tr>
                    <tr>

                      <td width="50%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <b><![CDATA[ ]]></b>
                        </p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" align="center" style='text-align:center;line-height:normal'>
                          <span style='font-size:8.0pt'>(подпись, дата)</span>
                        </p>
                      </td>
                      <td width="10%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'><![CDATA[ ]]></p>
                      </td>
                      <td width="20%" valign="top" style='border:none;padding:0cm 5.4pt 0cm 5.4pt'>
                        <p class="MsoNormal" style='line-height:normal'>
                          <span style='font-size:8.0pt'>(фамилия, инициалы)</span>
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