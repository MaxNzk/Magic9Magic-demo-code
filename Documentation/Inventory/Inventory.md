# Inventory/Market and Stats

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

	Located in "Features\Inventory\", "Features\Market\" and "Features\Inventory\Stats\".
	
	Stats uses +/- without percentages.
	
-------------------------------------------------------------------------------------------
	
**Spatial Ring:**

Silver rings always take up space in the slot[0] in the Market.

Golden rings always take up space in the slot[1] in the Market.

Black rings always take up space in the slot[2] in the Market.

Open slot amount is stored in AttackValues[0] and PlayerSettings.RingSlotAmount.

	silver open slots = 5 * ringLevel

	gold open slots = silver open slots + 1

	black open slots = silver open slots + 2

	gold ring price = siver ring price + (siver ring price / silver open slots) * 0.75

	black ring price = siver ring price + (siver ring price / silver open slots) * 0.75 * 2
	
-------------------------------------------------------------------------------------------

**Inventory:**

![Inventory button events](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Inventory/inventory01.jpg?raw=true)

![Inventory Market close button](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Inventory/inventory02.jpg?raw=true)

![Inventory uiManager](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Inventory/inventory03.jpg?raw=true)

![Inventory links](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Inventory/inventory04.jpg?raw=true)

-------------------------------------------------------------------------------------------

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
