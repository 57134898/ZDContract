# ZDContract
合同软件
升级日志
<br/>
1acontract 表增加字段 groupid
<br/>
2vcontract 视图增加字段 groupid ，groupname
<br/>
业务员表 SQL：
INSERT AYWY ( YCODE,YNAME )SELECT '1'+ RIGHT(YCODE,LEN(YCODE)-1),YNAME FROM AYWY WHERE SUBSTRING(YCODE,1,1)='0' AND YCODE!=''
<br/>
用户表 SQL:
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
添加bcode视图：
SELECT     bcode, bname, shortcode,replace(bname,'沈阳铸锻工业有限公司','') as shortname
FROM         n7_铸锻公司.dbo.bcode
WHERE     (LEN(bcode) <= 6)
