package crc6410c81121335e187f;


public class CalendarActivity
	extends crc6410c81121335e187f.BaseDrawerActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PhysioTherapyCenter.CalendarActivity, PhysioTherapyCenter", CalendarActivity.class, __md_methods);
	}


	public CalendarActivity ()
	{
		super ();
		if (getClass () == CalendarActivity.class)
			mono.android.TypeManager.Activate ("PhysioTherapyCenter.CalendarActivity, PhysioTherapyCenter", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
