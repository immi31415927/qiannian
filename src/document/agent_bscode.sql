/*
Navicat MySQL Data Transfer

Source Server         : 1000n
Source Server Version : 50614
Source Host           : 103.229.126.67:3306
Source Database       : 1000n

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2018-02-01 17:54:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for agent_bscode
-- ----------------------------
DROP TABLE IF EXISTS `agent_bscode`;
CREATE TABLE `agent_bscode` (
  `SysNo` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统编号',
  `Type` int(11) DEFAULT NULL COMMENT '类型',
  `Name` varchar(200) DEFAULT NULL COMMENT '名称',
  `Code` int(11) DEFAULT NULL COMMENT '代码',
  `Value` varchar(2000) DEFAULT NULL COMMENT '参数',
  `Remarks` varchar(300) DEFAULT NULL COMMENT '备注',
  `Status` int(11) DEFAULT NULL COMMENT '状态 启用（1）禁用（0）',
  `CreatedDate` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`SysNo`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8 COMMENT='码表';

-- ----------------------------
-- Records of agent_bscode
-- ----------------------------
INSERT INTO `agent_bscode` VALUES ('1', '10', '会员等级信息', null, '[{\"Type\":10,Name:\"普通会员\",\"Amount\":0},{\"Type\":20,Name:\"VIP\",\"Amount\":200},{\"Type\":30,Name:\"区域代理\",\"Amount\":900},{\"Type\":40,Name:\"全国代理\",\"Amount\":2300}]', '会员等级信息', '1', '2017-09-19 15:03:44');
INSERT INTO `agent_bscode` VALUES ('2', '20', '股池资金', null, '203770.0', '股池资金', '1', '2017-09-19 15:03:44');
INSERT INTO `agent_bscode` VALUES ('3', '30', '股权数总额', null, '202896', '股权数总额', '1', '2017-09-19 15:04:47');
INSERT INTO `agent_bscode` VALUES ('4', '40', '股权价格', null, '1.00430763', '股权价格（股池资金/股权数总额）', '1', '2017-09-19 15:04:47');
INSERT INTO `agent_bscode` VALUES ('5', '50', '股东见点奖', null, '[{\"Amount\":20,\"Level\":1},{\"Amount\":20,\"Level\":2},{\"Amount\":20,\"Level\":3},{\"Amount\":20,\"Level\":4},{\"Amount\":20,\"Level\":5},{\"Amount\":20,\"Level\":6},{\"Amount\":20,\"Level\":7},{\"Amount\":20,\"Level\":8},{\"Amount\":20,\"Level\":9},{\"Amount\":20,\"Level\":10}]', '股东见点奖', '1', '2017-09-19 15:05:39');
INSERT INTO `agent_bscode` VALUES ('6', '60', '代理销售奖金', null, '[{Grade:20,Bonus:100,Sort:1},{Grade:20,Bonus:50,Sort:2},{Grade:20,Bonus:50,Sort:3},{Grade:30,Bonus:500,Sort:1},{Grade:30,Bonus:100,Sort:2},{Grade:30,Bonus:100,Sort:3},{Grade:30,Bonus:50,Sort:4},{Grade:30,Bonus:50,Sort:5},{Grade:30,Bonus:50,Sort:6},{Grade:30,Bonus:50,Sort:7},{Grade:40,Bonus:1150,Sort:1},{Grade:40,Bonus:100,Sort:2},{Grade:40,Bonus:100,Sort:3},{Grade:40,Bonus:50,Sort:4},{Grade:40,Bonus:50,Sort:5},{Grade:40,Bonus:50,Sort:6},{Grade:40,Bonus:50,Sort:7},{Grade:40,Bonus:50,Sort:8},{Grade:40,Bonus:50,Sort:9},{Grade:40,Bonus:50,Sort:10},{Grade:40,Bonus:50,Sort:11},{Grade:40,Bonus:50,Sort:12},{Grade:40,Bonus:50,Sort:13},{Grade:40,Bonus:50,Sort:14},{Grade:40,Bonus:50,Sort:15},{Grade:40,Bonus:50,Sort:16},{Grade:40,Bonus:50,Sort:17},{Grade:40,Bonus:50,Sort:18},{Grade:40,Bonus:50,Sort:19},{Grade:40,Bonus:50,Sort:20},{Grade:40,Bonus:50,Sort:21},{Grade:40,Bonus:50,Sort:22}]', '代理销售奖金', '1', '2017-09-21 16:03:29');
INSERT INTO `agent_bscode` VALUES ('7', '70', '固定购股金额', null, '1500', '固定购股金额', '1', '2017-09-21 16:03:53');
INSERT INTO `agent_bscode` VALUES ('8', '80', '股权增值金额', null, '1300', '股权增值金额', '1', '2017-09-21 16:04:25');
INSERT INTO `agent_bscode` VALUES ('9', '90', '购股有效金额', null, '1000', '购股有效金额', '1', '2017-09-21 16:04:41');
INSERT INTO `agent_bscode` VALUES ('10', '100', '购股手续费率', null, '0.1', '购股手续费率', '1', '2017-09-21 16:04:57');
INSERT INTO `agent_bscode` VALUES ('11', '110', '推荐奖励', null, '500', '升级股东直系推荐人获得奖励', '1', '2017-09-26 14:19:53');
