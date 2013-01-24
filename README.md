DynamicRadGrid
==============

Demonstration of binding a dynamic class to a Telerik RadGrid control.

1/23/2013
Shows how much trouble dynamic classes cause to the in column filtering, the "funnel icon" in the column header.

1/24/2013
RadGrid filtering breaks down with dynamics and null values. Each column needs to have a value. Otherwise every row beneath can't be found by the RadGrid filter on that column.

The work around is to populate all columns, the drawback is the grid is no longer able to sort on null values.
