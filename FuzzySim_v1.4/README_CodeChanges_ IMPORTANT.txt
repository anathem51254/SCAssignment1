8:45 PM 16/10/2012
************************** Version 1.4

Fixed '-1 on DefuzzifyCOG' Error (updated DefuzzifyCOG method within Operations class). - TF 



5:15 PM 15/10/2012

************************** Version 1.3

Relative ship speed and initial velocity fixed. - RC, TF




2:55 AM 21/9/2012

**************************** Version 1.2

Hi guys, 

 Here is the FINAL VERSION of the FuzzySim application (I swear).

	Note:

		- Observe the new way to write Rules (not too different - just dont have to pass the rule itself
		by reference anymore! Lots of comments about this!

		- Added a 'TracePath' Layer to the Simulation - the AddTracePathBlip() method in the Simulator class 
		does a modulo on the turncount and plots the Lander's position and frame. (Render -> Trace Path)

		- Added Debug information to the Simulation - gives readouts of throttle settings next to the Lander 
		(Render -> Debug)

		- AI-Control is now a toggle - Default is on (Lander will crash initially, without any logic to apply...)
		
			** SELECT Reset Sim, then Untick the 'AutoPilot' checkbox, THEN tick 'Running' to fly by buttons

		- Control for Harrier:
			
			-A, B = Throttle Down / Up

			-E, F = Thrust Vector - / +

	Practice flying the Harrier by Buttons for a while - it will give you an appreciation for the sensitivity of the
	craft! 

 	Started y'all out with two Initialized RuleSet Collections (for Throttle and ThrustVector), 
	and with your Throttle and ThrustVector Accumulators (feel free to tinker/delete as you please)

	**Harrier Code is in Simulators/Harrier/HarrierController.cs

	**Harrier Variables are in SimVars


Happy Coding! Don't forget to email me with questions - tim.fox@live.com.au

		- Tim Fox
