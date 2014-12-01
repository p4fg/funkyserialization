Issue
======
The object serialized in huge.pbuf can be deserialized and re-serialized in about 6 seconds on my laptop using Serializer.Serialize().
Using Serializer.SerializeWithLengthPrefix() causes an apparent hang using 100% of one core.


The issue seems to be that the internal buffer is extended one or a few bytes at a time, and then copying a 1GB buffer to the extended buffer. So it is not a real hang, just something that will take a LOT of time to finish.

The object is arguably very big, but something is probably wrong as my computer has 32GB RAM is not swapping from what i can see in the resource manager.


Running the test
=================
* Unzip huge.zip to c:\temp (or somewhere else,just adjust the path in the unit-test).
* Make you are using 64-bit 

Experimenting
==============
(Just tested quicky so the timing is not very scientific...)

* Commenting out some of the attributes in SubItem so that only 2 remain, makes it finish in 10s.
* Commenting out some of the attributes in SubItem so that only 3 remain, makes it finish in 28s.
* Commenting out some of the attributes in SubItem so that only 4 remain, makes it finish in 28s.
* Commenting out some of the attributes in SubItem so that 5 remain, makes it finish in 28s.
* Using all six attributes it does not finish in resonable time...
