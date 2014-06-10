Add your own resources to ResourceRecovery as follows:


/////////////////////////////////
RESOURCE
{
  name = string 		- example: MyCool-Resource
  density = single 		- example: 0.005
  supply_mode = int  		- use one of these numbers: 0=unlimited;1=normal;2=recycle
  value_factor = decimal 	- example: 1 or 500 or 0.1
  display_order = int 		- example: 55 !!make sure you have no doubles!!
  hide = int-bool 		- example: 0=visible in flight mode; 1= hidden in flight mode
  group = string		- use one of these keywords: none|fuel|electric|air|waste|other
}
/////////////////////////////////


if you are not familiar with the terms above, google them to get all details..
In short it's:
string = a text value - a word - try not to use spaces and underscores here
single = a single precision float that is a number with many chars behind the dot
int = an integer - a number from 1-999 and more - Note: the script does not like single chars so use a trailing 0 for 1'9 like 01, 02, 04 etc..
int-bool = it's 0="no no" and 1="yes yes" - that simple
