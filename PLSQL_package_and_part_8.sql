






--PACKAGES
  -- Why?
  --  Global variable
  --  Overloading Procedure/function
  --  Performance of the system

--  Package has 2 parts
    -- Package Specification
    -- Package Body

--1/  Package Specification

    -- Syntax:
    --     CREATE OR REPLACE PACKAGE name_of_package IS
    --         global variable declaration
    --         global cursor   declaration
    --         Procedure / Function declaration
    --     END;
    --     /

--  Example 1:  Using schemas c##des02 (script 7clearwater),
--            Create a package specification with 2 globals variables
--     named  global_quantity and globale_inv_id of datatype number.
connect c##des02/tiger
CREATE OR REPLACE PACKAGE order_package IS
  global_inv_id NUMBER;
  global_quantity NUMBER;
END;
/

-- Example 2:  Create an anonimous block to assign number 10 to the global variable 
-- global_quantity,and number 32 to global variable global_inv_id.

  BEGIN
    order_package.global_inv_id := 32;
    order_package.global_quantity := 10;
  END;
  /
  
-- Example 3:  Create a procedure named print_global to print out the content of the
-- 2 global variables of the order_package as follow:
--  We would like to buy X of inventory number Y.
-- Where X = global_quantity , Y = global_inv_id

  CREATE OR REPLACE PROCEDURE print_global AS
    BEGIN
      DBMS_OUTPUT.PUT_LINE('We would like to buy '|| order_package.global_quantity ||
             ' of inventory number  ' || order_package.global_inv_id || '.'   );
    END;
    /
    
set serveroutput on
EXEC print_global

-- The above package can be called a BODYLESS package.
-- Example 4:  The client of application CLEARWATER would like to add a new order and
-- order line to the database.  Add procedures CREATE_NEW_ORDER that accepts the
-- c_id, method of payment, and the order source id to insert a new order and a new
-- order line using the values of the global variable

  desc orders 
     o_id    -- have to be auto generated by using a sequence
     c_id    -- supply on run time by the user
     os_id   -- supply on run time by the user
     o_date   we will  use sysdate
     o_methpmt  -- supply on run time by the user

  desc order_line
    inv_id     -- use the value  of global variable
    o_id
    ol_quantity -- use the value of global variable

CREATE OR REPLACE PACKAGE order_package IS
  global_inv_id NUMBER;
  global_quantity NUMBER;
  PROCEDURE create_new_order (p_customer_id NUMBER, p_meth_pmt VARCHAR2, p_os_id NUMBER);
  PROCEDURE create_new_order_line(p_order_id NUMBER);
END;
/
DESCRIBE order_package
    
--2/  Package BODY
--  SYNTAX:
--       CREATE OR REPLACE PACKAGE BODY name_of_package IS
--           private variable
--           unit programe code (code of PROCEDURE/FUNCTION)
--       END;
--       /

-- Example 5: Write the program code of the example 4.

CREATE SEQUENCE order_sequence START WITH 7;

CREATE OR REPLACE PACKAGE BODY order_package IS
  PROCEDURE create_new_order (p_customer_id NUMBER, p_meth_pmt VARCHAR2, p_os_id NUMBER) AS
      v_order_id NUMBER;
    BEGIN
      SELECT order_sequence.NEXTVAL 
      INTO   v_order_id
      FROM   dual;

      INSERT INTO orders
      VALUES(v_order_id,sysdate,p_meth_pmt,p_customer_id,p_os_id);

      CREATE_NEW_ORDER_LINE(v_order_id);
      COMMIT;
    END create_new_order;
    
  PROCEDURE create_new_order_line(p_order_id NUMBER)AS
    BEGIN
      INSERT INTO order_line
      VALUES(p_order_id,global_inv_id, global_quantity);
      COMMIT;
    END create_new_order_line;
END;
/

SELECT * FROM orders;
BEGIN
    order_package.global_inv_id := 31;
    order_package.global_quantity := 400;
  END;
  /

BEGIN
  order_package.create_new_order(3,'Peso',4);
END;
/

EXEC order_package.create_new_order(2,'FAVOR',6)






SELECT inv_id, inv_qoh FROM inventory WHERE inv_id = 25;


-- to remove the package body only
DROP PACKAGE BODY order_package;

DESC order_package

-- to remove the package completely
DROP PACKAGE order_package;

Be back by 9:10!!!!!!!!!!!!!!!!!


---------------------PROJECT PART 8 (3%)
Question 1: (use script 7clearwater)


Modify the package order_package (Example of lecture on PACKAGE) by adding 
function, procedure to verify the quantity on hand before insert a row in 
table order_line and to update also the quantity on hand of table inventory.


Do not create an order if the inv_qoh is ZERO.
Please use SALE ALL policy!


Test your package with different cases.



Question 2: (use script 7software)
Create a package with a procedure that accepts the consultant id, skill id,
 and a letter to insert a new row into table consultant_skill.



After the record is inserted, display the consultant last and first name, 
skill description and the status of the certification as CERTIFIED or 
Not Yet Certified.



Do not forget to handle the errors such as: Consultant, skill does not exist and 
the certification is different than 'Y' or 'N'.



Test your package at least 2 times!