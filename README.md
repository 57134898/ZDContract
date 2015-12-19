# ZDContract
合同软件
升级日志
1acontract 表增加字段 groupid
2vcontract 视图增加字段 groupid ，groupname

业务员表 SQL：
INSERT AYWY ( YCODE,YNAME )SELECT '1'+ RIGHT(YCODE,LEN(YCODE)-1),YNAME FROM AYWY WHERE SUBSTRING(YCODE,1,1)='0' AND YCODE!=''
