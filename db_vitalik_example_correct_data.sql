INSERT INTO `dx_article_attribute_translation` (`ARTICLE_ATTRIBUTE_ARTICLE_ID`, `ARTICLE_ATTRIBUTE_ATTRIBUTE_ID`, `LANGUAGE_ID`, `NOTES`, `VALUE`, `MODIF_DT`, `MODIF_USER_ID`, `LOCK_INFO`, `LOCK_ID`) 
VALUES
	(1, 1, 1, '423', '{"code": 1488, "name": 242}', NULL, NULL, NULL, NULL), --if multiple elements in "value" field
	(2, 2, 2, '423', '{"val": 242}', NULL, NULL, NULL, NULL); --if single element in "value" field
	