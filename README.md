# ZDContract
合同软件
升级日志
<p>1业务员表 SQL：</p>
INSERT AYWY (YCODE,YNAME)
SELECT '02'+ RIGHT(YCODE,LEN(YCODE)-2),
            YNAME
FROM AYWY
WHERE SUBSTRING(YCODE,1,2)='01'
  AND YCODE!=''
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
<p>3添加bcode视图：</p>
SELECT     bcode, bname, shortcode,replace(bname,'沈阳铸锻工业有限公司','') as shortname
FROM         n7_铸锻公司.dbo.bcode
WHERE     (LEN(bcode) <= 6)
<p>5VCONTRACT视图：</p>
SELECT dbo.ACONTRACT.HXM AS 项目名称, dbo.ACLIENTS.CNAME AS 客户名, dbo.ACONTRACT.HCODE AS 合同号, dbo.ACONTRACT.HDATE AS 签定日期, dbo.ACONTRACT.HHTJE AS 合同金额, dbo.ACONTRACT.HJSJE AS 结算金额, dbo.ACONTRACT.HZBJ AS 质保金, dbo.ACONTRACT.HYF AS 运费, dbo.ACONTRACT.HQTFY AS 其它费用, dbo.ALX.LNAME AS 合同类型, dbo.ACONTRACT.RMB AS 金额, dbo.ACONTRACT.FP AS 发票, dbo.ACONTRACT.HJSJE - dbo.ACONTRACT.RMB AS 金额1, dbo.ACONTRACT.HJSJE - dbo.ACONTRACT.FP AS 发票1, dbo.ACONTRACT.GY AS 估验, dbo.ACONTRACT.FP - dbo.ACONTRACT.RMB AS 财务余额, dbo.AREAS.ANAME AS 地区, dbo.ACONTRACT.HMEMO AS 合同备注, dbo.AYWY.YNAME AS 业务员, dbo.ACONTRACT.HZT AS 状态, dbo.ACONTRACT.HJHDATE AS 交货日期, dbo.ACONTRACT.HUSER AS 操作员, dbo.ACONTRACT.HYHMC AS 银行名称, dbo.ACONTRACT.HYHZH AS 账号, dbo.ACONTRACT.HWTR1 AS 对方委托人, dbo.ACONTRACT.HWTR2 AS 己方委托人, dbo.ACONTRACT.HHS AS 含税, dbo.ACONTRACT.HHSBL AS 比例, dbo.ACONTRACT.PS1 AS 部门主管, dbo.ACONTRACT.PS2 AS 财务主管, dbo.ACONTRACT.PS3 AS 公司主管, dbo.ACONTRACT.PS4 AS 审计主管, dbo.ACONTRACT.YJ1 AS 部门意见, dbo.ACONTRACT.YJ2 AS 财务意见, dbo.ACONTRACT.YJ3 AS 公司意见, dbo.ACONTRACT.YJ4 AS 审计意见, dbo.ACONTRACT.MACODE, dbo.ACONTRACT.MBCODE, dbo.ACONTRACT.MCCODE, dbo.ACONTRACT.MDCODE, dbo.ACONTRACT.MECODE, dbo.ACONTRACT.MFCODE, dbo.ACONTRACT.MGCODE, dbo.ACONTRACT.FLAG, dbo.ACONTRACT.HAREA, dbo.ACONTRACT.HLX, dbo.ACONTRACT.HKH, dbo.ACONTRACT.HYWY, dbo.ACONTRACT.HDW, dbo.ACONTRACT.HID, dbo.ACONTRACT.DLF AS 代理费, dbo.ACONTRACT.XXF AS 选型费, dbo.ACONTRACT.BSF AS 标书费, T1.BNAME AS 公司名, dbo.ACONTRACT.QDRQ AS 签订日期, dbo.ACONTRACT.ZBFS AS 中标方式, dbo.ACONTRACT.WXState, dbo.ACONTRACT.BIDCODE AS 标号 FROM dbo.ACONTRACT LEFT OUTER JOIN dbo.ACLIENTS ON dbo.ACONTRACT.HKH = dbo.ACLIENTS.CCODE LEFT OUTER JOIN dbo.AREAS ON dbo.ACONTRACT.HAREA = dbo.AREAS.ACODE LEFT OUTER JOIN dbo.AYWY ON dbo.ACONTRACT.HYWY = dbo.AYWY.YCODE LEFT OUTER JOIN dbo.ALX ON dbo.ACONTRACT.HLX = dbo.ALX.LID INNER JOIN dbo.BCODE AS T1 ON T1.bcode = dbo.ACONTRACT.HDW
