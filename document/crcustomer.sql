/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50717
Source Host           : 127.0.0.1:3306
Source Database       : 1000n

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2018-05-07 19:03:15
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for crcustomer
-- ----------------------------
DROP TABLE IF EXISTS `crcustomer`;
CREATE TABLE `crcustomer` (
  `SysNo` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统编号',
  `Account` varchar(50) DEFAULT NULL COMMENT '账号',
  `Password` varchar(64) DEFAULT NULL COMMENT '密码',
  `SerialNumber` varchar(32) DEFAULT NULL COMMENT '编码',
  `OpenId` varchar(64) DEFAULT NULL COMMENT 'OpenId',
  `ReferrerSysNo` int(11) DEFAULT NULL COMMENT '经理编号',
  `PhoneNumber` varchar(11) DEFAULT NULL,
  `RealName` varchar(20) DEFAULT NULL COMMENT '真实名称',
  `Nickname` varchar(20) DEFAULT NULL COMMENT '呢称',
  `HeadImgUrl` varchar(200) DEFAULT NULL COMMENT '头像',
  `IDNumber` varchar(20) DEFAULT NULL COMMENT '身份证',
  `Email` varchar(50) DEFAULT NULL COMMENT '邮箱',
  `TeamCount` int(11) DEFAULT NULL COMMENT '团队人数',
  `Grade` int(11) DEFAULT NULL COMMENT '会员等级（VIP会员(10)  普通代理 (20)  区域代理 (30)  全国代理 (40)  股东 (50)）',
  `Level` int(11) DEFAULT NULL COMMENT '层级',
  `LevelCustomerStr` varchar(2000) DEFAULT NULL COMMENT '层级推荐会员（该会员所以层级推荐人编号（以逗号隔开 如：1,2,3））',
  `Bank` int(11) DEFAULT NULL COMMENT '银行',
  `BankNumber` varchar(20) DEFAULT NULL COMMENT '银行卡号',
  `WalletAmount` decimal(38,8) DEFAULT NULL COMMENT '奖金',
  `HistoryWalletAmount` decimal(38,8) DEFAULT NULL COMMENT '历史钱包',
  `WithdrawalsTotalAmount` decimal(38,8) DEFAULT '0.00000000' COMMENT '提现总金额',
  `UpgradeFundAmount` decimal(38,8) DEFAULT NULL COMMENT '升级基金',
  `RechargeTotalAmount` decimal(38,8) DEFAULT NULL COMMENT '充值总金额',
  `GeneralBonus` decimal(38,8) DEFAULT NULL COMMENT '普通待结算奖金',
  `AreaBonus` decimal(38,8) DEFAULT NULL COMMENT '区域待结算奖金',
  `GlobalBonus` decimal(38,8) DEFAULT NULL COMMENT '全国待结算奖金',
  `ExpiresTime` varchar(20) DEFAULT NULL COMMENT '过期日期',
  `FollowDate` varchar(20) DEFAULT NULL COMMENT '关注时间',
  `RegisterIP` varchar(20) DEFAULT NULL COMMENT '注册IP',
  `RegisterDate` datetime DEFAULT NULL COMMENT '注册时间',
  `LastLoginIP` varchar(20) DEFAULT NULL COMMENT '最后登录IP',
  `LastLoginDate` datetime DEFAULT NULL COMMENT '最后登陆时间',
  `LoginCount` int(11) DEFAULT NULL COMMENT '登录次数',
  `Status` int(11) DEFAULT NULL COMMENT '状态：启用（1）、禁用（0）',
  `CreatedDate` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`SysNo`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8 COMMENT='会员';

-- ----------------------------
-- Records of crcustomer
-- ----------------------------
INSERT INTO `crcustomer` VALUES ('1', '18818818800', '3d59d30c41b9330f4b6a6593b683e8a5:c2', '89490578', '1', '0', '18818818800', '千年教育', '千年教育', '', '', null, '145', '40', '0', '', '0', null, '75.00000000', '75.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '171.210.205.67', '2008-08-08 13:36:14', '171.210.47.22', '2018-04-18 14:28:37', '1', '1', '2018-03-14 13:36:14');
INSERT INTO `crcustomer` VALUES ('2', '13547777888', 'd627d0385e4ae643b6941a8d5e63a74f:bd', '43417600', '2', '1', '13547777888', '唐跃武', '羞花拉西', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Nibq7wHsJvqxL2ZmcgLhmYepwndp2TSOicyiaaw5HNxibMmLLNdH6gibrOC53754ibahVLvLf0xhq1cxqmxftIee6XAQ/132', '', null, '106', '30', '1', '26', '1', null, '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '223.86.106.120', '2018-04-18 14:16:30', '117.136.62.115', '2018-04-26 11:47:18', '1', '1', '2018-04-18 14:16:30');
INSERT INTO `crcustomer` VALUES ('3', '14780081303', '62701eac5a55685595a72b2c71db9960:ec', '22810727', '3', '2', '14780081303', '陈钱', '近在眼前', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKWyf1fLhoSR9GFxSXibRJ0cPhzyVD4EN3uwz3OuUI8p4clmQKgPnySW2UtgBictOsicqJjZh4wnpsDg/132', '', null, '47', '40', '2', '26,63', '1', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '223.86.106.120', '2018-04-18 14:18:19', '117.172.202.84', '2018-04-26 10:46:25', '1', '1', '2018-04-18 14:18:19');
INSERT INTO `crcustomer` VALUES ('4', '13111899668', '0325fd71fa4173caea799d7d09887ff4:d3', '67471109', '4', '3', '13111899668', '杨星', '中國制造', 'http://thirdwx.qlogo.cn/mmopen/vi_32/ubxMYvxbqXjhCQ0wjwNLdh8KqejTrKvmz6H0oeA9G0zbhUaDvrp8VcJTPkVY7VBPzGZ3T1zHxpkzaqusepKO6w/132', '', null, '10', '40', '1', '26', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '124.161.223.171', '2018-04-18 14:20:02', '139.207.25.237', '2018-04-25 22:38:54', '1', '1', '2018-04-18 14:20:02');
INSERT INTO `crcustomer` VALUES ('5', '18582504991', 'b40d7398db325c03dfb31d0ac88a4f74:3a', '78166615', '5', '4', '18582504991', '肖锦华', '肖锦华', 'http://thirdwx.qlogo.cn/mmopen/vi_32/HMQoQAZ1IibXQD9P9bwicc6MK2qJN4YLyOibicmSq99QkLSSIKQxTUZJvN4RH5mxlicSTsV6JibQyYU8MxUVw8sM2I7w/132', '', null, '0', '40', '2', '26,65', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '124.161.223.171', '2018-04-18 14:20:28', '118.113.1.186', '2018-04-21 20:13:10', '1', '1', '2018-04-18 14:20:28');
INSERT INTO `crcustomer` VALUES ('6', '18728597373', '68bf9133e5cf69c410f951cd80fa7a07:26', '21815083', '6', '5', '18728597373', '陈朋', '中医秘方    陈朋', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKia6yoCoJFiac5837hMseOBrj0TRia6CdNukmor4kpiaWUJqdYdn1VcB4Gl88IWtTiax5EoIBvX5A8kLg/132', '', null, '15', '40', '3', '26,63,64', '2', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '223.86.106.120', '2018-04-18 14:31:26', '117.136.70.254', '2018-04-25 22:01:02', '1', '1', '2018-04-18 14:31:26');
INSERT INTO `crcustomer` VALUES ('7', '15008228718', '0b2ce06f230f6a078806e9b8d78f3cd7:40', '32609310', '7', '6', '15008228718', '苟治国', 'IMMI', 'http://thirdwx.qlogo.cn/mmopen/vi_32/R1DkoMQ7BNPP7thx6KheGCVKREMZkJCOZvRfeJibTIya5xkyqP1iaqupaXVC9lwuszJuqic1mmSPSM0jFIqeibMbPw/132', '', null, '0', '40', '0', '', '1', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '58.71.29.164', '2018-04-18 21:38:16', '116.93.126.166', '2018-04-26 11:27:27', '1', '1', '2018-04-18 21:38:16');
INSERT INTO `crcustomer` VALUES ('8', '15228681267', 'cb5a598156083df9f78e1b2f8d240bf4:bb', '67899968', '8', '7', '15228681267', '向丽', '永恒的微笑', 'http://thirdwx.qlogo.cn/mmopen/vi_32/qmCibC5BQibBkicxEqrcvriankUeBB7ncxkSWqTBhcD6s6lJmYhEfZgGoFVdc8IJxZibhD97R3hZ9iaaYDc2WuFfCYAg/132', '', null, '6', '40', '4', '26,63,64,67', '1', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.136.62.120', '2018-04-19 05:52:12', '117.136.62.134', '2018-04-25 14:32:16', '1', '1', '2018-04-19 05:52:12');
INSERT INTO `crcustomer` VALUES ('9', '18228929008', '1ad27cbb52cae61e0592996d6b74fdca:2d', '33044922', '9', '8', '18228929008', '代丽', '代丽', 'http://thirdwx.qlogo.cn/mmopen/vi_32/ebR3q0aEjPZa6nsSrB6fxL68AdrGA8HdUhPvNUX8fF1LFZdKe1IwOT1oMToqmMELicagXDxeI6EyNOV76dhKtNw/132', '', null, '1', '40', '4', '26,63,64,67', '3', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '110.185.146.35', '2018-04-19 06:47:19', '124.161.223.166', '2018-04-24 08:16:41', '1', '1', '2018-04-19 06:47:19');
INSERT INTO `crcustomer` VALUES ('10', '18982571480', '77ff67fc5db73f6872bde6447fef9e42:1e', '89070609', '10', '9', '18982571480', '蔡艳', null, null, '', null, '0', '40', '0', '', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '171.208.160.2', '2018-04-19 07:12:40', null, null, '1', '1', '2018-04-19 07:12:40');
INSERT INTO `crcustomer` VALUES ('11', '19908253600', 'ef4f15bbcb7d76be2f9f7dcdac9eff33:dd', '22309009', '11', '10', '19908253600', '张露莹', '神奇的老杨', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKIZVQwX01EgDv9HcIbezOEkia2IFhWjs14fLyBUJlqrKjnYAB2CQibHzI8wmNRZzayhxdSxDXqkrFw/132', '', null, '7', '40', '2', '26,65', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '139.207.118.60', '2018-04-19 10:33:34', '119.7.162.7', '2018-04-22 21:38:39', '1', '1', '2018-04-19 10:33:34');
INSERT INTO `crcustomer` VALUES ('12', '18282590505', '7899b9a0e090a1a99052c42c9685e5d0:3d', '44853432', '12', '11', '18282590505', '石艳', '石艳', 'http://thirdwx.qlogo.cn/mmopen/vi_32/ajzEk7HEy9VqjuR3la964x9RNw0wctrVI0FJH2vibfcxEcYv0IviagnxQApx042oIPrftkTy26fyz6AmtJtESu2g/132', '', null, '19', '40', '3', '26,63,64', '2', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '111.17.180.202', '2018-04-19 10:58:08', '222.209.25.255', '2018-04-25 08:03:25', '1', '1', '2018-04-19 10:58:08');
INSERT INTO `crcustomer` VALUES ('13', '15928908633', '626b572bbdfbf4528e5fa1a5596d53f4:e5', '89101099', '13', '12', '15928908633', '袁欣', '袁欣', 'http://thirdwx.qlogo.cn/mmopen/vi_32/W9dX1T1pOGZT7JRVOibcAeoheiaQ2yMFsJWLzR4X42Jj7IdhWX905guSXMa1FAGg6YL3MHa2JzJNqNDaaLCBJR8Q/132', '', null, '0', '40', '4', '26,63,64,73', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.136.77.158', '2018-04-19 12:28:06', '223.104.185.142', '2018-04-23 14:42:54', '1', '1', '2018-04-19 12:28:06');
INSERT INTO `crcustomer` VALUES ('14', '15328794068', 'b6f7069002c4a6192e6496b92a597fd7:6b', '55573739', '14', '13', '15328794068', '可可', null, null, '', null, '0', '40', '0', '', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.177.199.141', '2018-04-19 12:48:39', null, null, '1', '1', '2018-04-19 12:48:39');
INSERT INTO `crcustomer` VALUES ('15', '15739854222', '44584e58bbf4f37d7780919b2b15d71e:92', '10596881', '15', '14', '15739854222', '陈柏华', '忧伤的天使', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTJT7F5DBVfdPfVyPonynrlnabFyMHjWo2gzd5boDLf7rQduGdHjsNwFiaiaEjxIicc9M3WQkbKOYiafIw/132', '', null, '0', '40', '4', '26,63,64,67', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '124.161.223.171', '2018-04-19 13:37:26', '124.161.223.171', '2018-04-19 13:55:04', '1', '1', '2018-04-19 13:37:26');
INSERT INTO `crcustomer` VALUES ('16', '15506764440', '63ac5474efc297359e33d3b78796111e:c9', '10842937', '16', '15', '15506764440', '唐长福', '寂寞天使', 'http://thirdwx.qlogo.cn/mmopen/vi_32/A94RKUfWfwwIY34QPfUqxCZ04EO2znibibDkuibUCm4Vu90aA5AGwQBcobtfjvcxxvILWT0XjC7LycWg66Qpq8TwA/132', '', null, '0', '40', '3', '26,63,64', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.136.65.145', '2018-04-19 13:51:19', '117.136.65.145', '2018-04-19 13:51:57', '1', '1', '2018-04-19 13:51:19');
INSERT INTO `crcustomer` VALUES ('17', '18784613880', 'c1334949644b2be3f601a37046629c81:fc', '10370461', '17', '16', '18784613880', '何刚', '、', 'http://thirdwx.qlogo.cn/mmopen/vi_32/DYAIOgq83eqhwia4KwiarIz9g0I99Zs780b5esPo7ULtC63RgrJ2myFNZIvdjR3GiaoUiazLXTs5hicv7Sl4tKfj3qQ/132', '', null, '0', '40', '0', '', '0', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.136.62.225', '2018-04-19 14:06:12', '117.136.62.211', '2018-04-19 23:49:16', '1', '1', '2018-04-19 14:06:12');
INSERT INTO `crcustomer` VALUES ('18', '15908306566', '8dbf1d6a7d1b9bc4a3b4408d19b37d5a:bb', '10547363', '18', '17', '15908306566', '郭玉秀', '小黄豆琴行', 'http://thirdwx.qlogo.cn/mmopen/vi_32/eUbAz0bGEtmte1Dp8EwP2WkeKhZLGO7OU3N260LQGn5Qkia4350ukX7zpPBhvm7ib7rEMibuwg9ibhdC4mibvHfxYNQ/132', '', null, '51', '40', '2', '26,63', '1', null, '50.00000000', '50.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '117.136.63.199', '2018-04-19 14:06:31', '223.104.217.32', '2018-04-26 11:30:08', '1', '1', '2018-04-19 14:06:31');
INSERT INTO `crcustomer` VALUES ('19', '18782566607', '770730e50e97c47fb3bc86f172125dc4:0c', '55549524', '19', '18', '18782566607', '唐月稀', 'Tyuexi ', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKVRwtvDSFQd3RxiaCFfuzDtd2YyTvT54wxGMib25mRaQFYT7XVQnZ0c8dia6PLOoRcAUHchOZQVvJAg/132', '', null, '29', '40', '3', '26,63,79', '4', null, '100.00000000', '100.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '182.139.121.64', '2018-04-19 14:25:55', '183.222.147.56', '2018-04-26 08:03:36', '1', '1', '2018-04-19 14:25:55');
INSERT INTO `crcustomer` VALUES ('20', '15282575866', '4774b29cf65c9ca4dd5112887b713e55:c0', '21491300', '20', '19', '15282575866', '未昌东', '东哥', '', '', null, '3', '40', '3', '26,63,64', '0', null, '100.00000000', '100.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '124.161.223.171', '2018-04-19 14:38:54', '117.136.63.130', '2018-04-25 10:56:21', '1', '1', '2018-04-19 14:38:54');
INSERT INTO `crcustomer` VALUES ('21', '18792541322', '87c254f9dfef5e7fd9597620bc3f7cfc:9f', '89169416', '21', '20', '18792541322', '唐利萍', '糖豆豆', 'http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKlDrJmVufPfDgovHI8XtjHfcicB8SNiaOnrr8Og6Yx2ZFWbOf6sPlGZQOAqdthno3QpRLvCU2jQyEA/132', '', null, '28', '40', '4', '26,63,79,80', '2', null, '400.00000000', '400.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', null, null, '223.104.11.21', '2018-04-19 14:52:52', '222.42.235.14', '2018-04-26 06:35:35', '1', '1', '2018-04-19 14:52:52');
INSERT INTO `crcustomer` VALUES ('22', '15578564582', '2d3d7bf7cf1a1f59a573370b8b68a05f:5f', '89783463', '22', '21', '15578564582', '周三', null, null, '', null, '0', '40', '2', '26,65', '0', null, '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '0.00000000', '2019-05-07', null, '112.45.225.185', '2018-04-19 14:59:42', '::1', '2018-05-07 16:15:00', '1', '1', '2018-04-19 14:59:42');
