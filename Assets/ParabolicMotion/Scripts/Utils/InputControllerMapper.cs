using System.Collections.Generic;

public static class InputControllerMapper
{
    public static Dictionary<EButtonInputController, Dictionary<EHandSide, OVRInput.Button>> ButtonInputControllerMapper = new Dictionary<EButtonInputController, Dictionary<EHandSide, OVRInput.Button>>() {
	    {
		    EButtonInputController.THUMBSTICK, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT , OVRInput.Button.PrimaryThumbstick },
			    { EHandSide.RIGHT, OVRInput.Button.SecondaryThumbstick }
		    }
	    }, {
			EButtonInputController.ONE, new Dictionary<EHandSide, OVRInput.Button>() {
				{ EHandSide.LEFT, OVRInput.Button.None },
				{ EHandSide.RIGHT, OVRInput.Button.One }
			}
	    }, {
		    EButtonInputController.TWO, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT, OVRInput.Button.None },
			    { EHandSide.RIGHT, OVRInput.Button.Two }
		    }
	    }, {
		    EButtonInputController.THREE, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT, OVRInput.Button.Three },
			    { EHandSide.RIGHT, OVRInput.Button.None }
		    }
	    }, {
		    EButtonInputController.FOUR, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT, OVRInput.Button.Four },
			    { EHandSide.RIGHT, OVRInput.Button.None }
		    }
	    }, {
		    EButtonInputController.TRIGGER, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT, OVRInput.Button.PrimaryIndexTrigger },
			    { EHandSide.RIGHT, OVRInput.Button.SecondaryIndexTrigger }
		    }
	    }, {
		    EButtonInputController.HANDTRIGGER, new Dictionary<EHandSide, OVRInput.Button>() {
			    { EHandSide.LEFT, OVRInput.Button.PrimaryHandTrigger },
			    { EHandSide.RIGHT, OVRInput.Button.SecondaryHandTrigger }
		    }
	    },
	};
}
