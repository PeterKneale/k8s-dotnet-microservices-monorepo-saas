CREATE VIEW category_view
AS
WITH RECURSIVE nodes AS (
    SELECT s1.account_id,
           s1.category_id,
           s1.parent_category_id,
           s1.name,
           0 as              level,
           category_id::TEXT id_path,
           name::TEXT        name_path
    FROM category s1
    WHERE parent_category_id is null
    UNION
    SELECT s2.account_id,
           s2.category_id,
           s2.parent_category_id,
           s2.name,
           level + 1,
           s1.id_path || ',' || s2.category_id,
           s1.name_path || ',' || s2.name
    FROM category s2,
         nodes s1
    WHERE s2.parent_category_id = s1.category_id
)
SELECT *
FROM nodes
ORDER BY name_path;