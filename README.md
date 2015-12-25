# ZDContract
合同软件
升级日志
<br/>
<p>1业务员表 SQL：</p>
INSERT AYWY (YCODE,YNAME)
SELECT '02'+ RIGHT(YCODE,LEN(YCODE)-2),
            YNAME
FROM AYWY
WHERE SUBSTRING(YCODE,1,2)='01'
  AND YCODE!=''
<br/>
<p>2用户表 SQL:</p>
INSERT AUSERS([UCODE],[UNAME],[UPASSWORD],[UDW],[UBM],[POS],[AUT],[FLAG],[PCODE])
SELECT '02'+ RIGHT(UCODE,LEN(UCODE)-2),
            UNAME,
            UPASSWORD,
            CASE
                WHEN UDW IS NULL
                     OR UDW = '' THEN NULL
                ELSE '02'+ RIGHT(UDW,LEN(UDW)-2)
            END,
            CASE
                WHEN UBM IS NULL
                     OR UBM = '' THEN NULL
                ELSE '02'+ RIGHT(UBM,LEN(UBM)-2)
            END,
            POS,
            AUT,
            FLAG,
            PCODE
FROM AUSERS
WHERE SUBSTRING(UCODE,1,2)='01' AND UCODE!=''
<br/>
<p>3添加bcode视图：</p>
SELECT     bcode, bname, shortcode,replace(bname,'沈阳铸锻工业有限公司','') as shortname
FROM         n7_铸锻公司.dbo.bcode
WHERE     (LEN(bcode) <= 6)
