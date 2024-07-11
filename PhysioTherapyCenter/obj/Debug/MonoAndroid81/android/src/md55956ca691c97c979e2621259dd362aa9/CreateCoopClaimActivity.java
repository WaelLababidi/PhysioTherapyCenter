package md55956ca691c97c979e2621259dd362aa9;


public class CreateCoopClaimActivity
	extends md55956ca691c97c979e2621259dd362aa9.BaseDrawerActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onBackPressed:()V:GetOnBackPressedHandler\n" +
			"";
		mono.android.Runtime.register ("PhysioTherapyCenter.CreateCoopClaimActivity, PhysioTherapyCenter", CreateCoopClaimActivity.class, __md_methods);
	}


	public CreateCoopClaimActivity ()
	{
		super ();
		if (getClass () == CreateCoopClaimActivity.class)
			mono.android.TypeManager.Activate ("PhysioTherapyCenter.CreateCoopClaimActivity, PhysioTherapyCenter", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onBackPressed ()
	{
		n_onBackPressed ();
	}

	private native void n_onBackPressed ();

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
