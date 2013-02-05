//========================================//
// iRduino - Created by Mark Silverwood  //
//======================================//

namespace iRduino.Classes
{
    using System;
    using System.Collections.Generic;
    using iRacingSdkWrapper;

    public enum DCVariablesEnum
    {
        BrakeBias,
        AntiRollFront,
        AntiRollRear,
        FuelMixture,
        RevLimiter,
        WeightJackerLeft,
        WeightJackerRight,
        ABS,
        TractionControl,
        TractionControl2,
        WingFront,
        WingRear,
        DiffEntry,
        DiffMiddle,
        DiffExit,
        EngineBraking,
        EnginePower,
        ThrottleShape,
        DpFWingIndex,
        DpRWingIndex,
        DpRWingSetting,
        DpWedgeAdj,
        DpPSSetting,
        DpRRDamperPerchOffsetm,
        DpRBarSetting,
        DpRFTruckarmP1Dz,
        DpTruckarmP1Dz,
        DpQtape,
        DpFNOMKnobSetting
    }

    public class DCVariables
    {
        public Dictionary<DCVariablesEnum, bool> Available = new Dictionary<DCVariablesEnum, bool>();

        public DCVariables()
        {
            foreach (object dcV in Enum.GetValues(typeof (DCVariablesEnum)))
            {
                this.Available.Add((DCVariablesEnum) dcV, false);
            }
        }
    }

    public class DCVariablesFunctions
    {
        public static void GetDCVariableValue(Dictionary<DCVariablesEnum, float> dcVars, SdkWrapper.TelemetryUpdatedEventArgs e, DCVariablesEnum key)
        {
            switch (key)
            {
                case DCVariablesEnum.BrakeBias:
                    dcVars[key] = e.TelemetryInfo.dcBrakeBias.Value;
                    break;
                case DCVariablesEnum.AntiRollFront:
                    dcVars[key] = e.TelemetryInfo.dcAntiRollFront.Value;
                    break;
                case DCVariablesEnum.AntiRollRear:
                    dcVars[key] = e.TelemetryInfo.dcAntiRollRear.Value;
                    break;
                case DCVariablesEnum.FuelMixture:
                    dcVars[key] = e.TelemetryInfo.dcFuelMixture.Value;
                    break;
                case DCVariablesEnum.RevLimiter:
                    dcVars[key] = e.TelemetryInfo.dcRevLimiter.Value;
                    break;
                case DCVariablesEnum.WeightJackerLeft:
                    dcVars[key] = e.TelemetryInfo.dcWeightJackerLeft.Value;
                    break;
                case DCVariablesEnum.WeightJackerRight:
                    dcVars[key] = e.TelemetryInfo.dcWeightJackerRight.Value;
                    break;
                case DCVariablesEnum.ABS:
                    dcVars[key] = e.TelemetryInfo.dcABS.Value;
                    break;
                case DCVariablesEnum.TractionControl:
                    dcVars[key] = e.TelemetryInfo.dcTractionControl.Value;
                    break;
                case DCVariablesEnum.TractionControl2:
                    dcVars[key] = e.TelemetryInfo.dcTractionControl2.Value;
                    break;
                case DCVariablesEnum.WingFront:
                    dcVars[key] = e.TelemetryInfo.dcWingFront.Value;
                    break;
                case DCVariablesEnum.WingRear:
                    dcVars[key] = e.TelemetryInfo.dcWingRear.Value;
                    break;
                case DCVariablesEnum.DiffEntry:
                    dcVars[key] = e.TelemetryInfo.dcDiffEntry.Value;
                    break;
                case DCVariablesEnum.DiffMiddle:
                    dcVars[key] = e.TelemetryInfo.dcDiffMiddle.Value;
                    break;
                case DCVariablesEnum.DiffExit:
                    dcVars[key] = e.TelemetryInfo.dcDiffExit.Value;
                    break;
                case DCVariablesEnum.EngineBraking:
                    dcVars[key] = e.TelemetryInfo.dcEngineBraking.Value;
                    break;
                case DCVariablesEnum.EnginePower:
                    dcVars[key] = e.TelemetryInfo.dcEnginePower.Value;
                    break;
                case DCVariablesEnum.ThrottleShape:
                    dcVars[key] = e.TelemetryInfo.dcThrottleShape.Value;
                    break;
                case DCVariablesEnum.DpFWingIndex:
                    dcVars[key] = e.TelemetryInfo.dpFWingIndex.Value;
                    break;
                case DCVariablesEnum.DpRWingIndex:
                    dcVars[key] = e.TelemetryInfo.dpRWingIndex.Value;
                    break;
                case DCVariablesEnum.DpRWingSetting:
                    dcVars[key] = e.TelemetryInfo.dpRWingSetting.Value;
                    break;
                case DCVariablesEnum.DpWedgeAdj:
                    dcVars[key] = e.TelemetryInfo.dpWedgeAdj.Value;
                    break;
                case DCVariablesEnum.DpPSSetting:
                    dcVars[key] = e.TelemetryInfo.dpPSSetting.Value;
                    break;
                case DCVariablesEnum.DpRRDamperPerchOffsetm:
                    dcVars[key] = e.TelemetryInfo.dpRRDamperPerchOffsetm.Value;
                    break;
                case DCVariablesEnum.DpRBarSetting:
                    dcVars[key] = e.TelemetryInfo.dpRBarSetting.Value;
                    break;
                case DCVariablesEnum.DpRFTruckarmP1Dz:
                    dcVars[key] = e.TelemetryInfo.dpRFTruckarmP1Dz.Value;
                    break;
                case DCVariablesEnum.DpTruckarmP1Dz:
                    dcVars[key] = e.TelemetryInfo.dpTruckarmP1Dz.Value;
                    break;
                case DCVariablesEnum.DpQtape:
                    dcVars[key] = e.TelemetryInfo.dpQtape.Value;
                    break;
                case DCVariablesEnum.DpFNOMKnobSetting:
                    dcVars[key] = e.TelemetryInfo.dpFNOMKnobSetting.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Dictionary<DCVariablesEnum, float> SetDCList(SdkWrapper wrapper)
        {
            var dcVars = new Dictionary<DCVariablesEnum, float>();
            if (wrapper.IsConnected)
            {
                foreach (object dcV in Enum.GetValues(typeof(DCVariablesEnum)))
                {
                    try
                    {
                        var temp = (DCVariablesEnum)dcV;
                        switch (temp)
                        {
                            case DCVariablesEnum.BrakeBias:
                                if (wrapper.containsKey("dcBrakeBias"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.AntiRollFront:
                                if (wrapper.containsKey("dcAntiRollFront"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.AntiRollRear:
                                if (wrapper.containsKey("dcAntiRollRear"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.FuelMixture:
                                if (wrapper.containsKey("dcFuelMixture"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.RevLimiter:
                                if (wrapper.containsKey("dcRevLimiter"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.WeightJackerLeft:
                                if (wrapper.containsKey("dcWeightJackerLeft"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.WeightJackerRight:
                                if (wrapper.containsKey("dcWeightJackerRight"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.ABS:
                                if (wrapper.containsKey("dcABS"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.TractionControl:
                                if (wrapper.containsKey("dcTractionControl"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.TractionControl2:
                                if (wrapper.containsKey("dcTractionControl2"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.WingFront:
                                if (wrapper.containsKey("dcWingFront"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.WingRear:
                                if (wrapper.containsKey("dcWingRear"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DiffEntry:
                                if (wrapper.containsKey("dcDiffEntry"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DiffMiddle:
                                if (wrapper.containsKey("dcDiffMiddle"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DiffExit:
                                if (wrapper.containsKey("dcDiffExit"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.EngineBraking:
                                if (wrapper.containsKey("dcEngineBraking"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.EnginePower:
                                if (wrapper.containsKey("dcEnginePower"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.ThrottleShape:
                                if (wrapper.containsKey("dcThrottleShape"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpFWingIndex:
                                if (wrapper.containsKey("dpFWingIndex"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpRWingIndex:
                                if (wrapper.containsKey("dpRWingIndex"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpRWingSetting:
                                if (wrapper.containsKey("dpRWingSetting"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpWedgeAdj:
                                if (wrapper.containsKey("dpWedgeAdj"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpPSSetting:
                                if (wrapper.containsKey("dpPSSetting"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpRRDamperPerchOffsetm:
                                if (wrapper.containsKey("dpRRDamperPerchOffsetm"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpRBarSetting:
                                if (wrapper.containsKey("dpRBarSetting"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpRFTruckarmP1Dz:
                                if (wrapper.containsKey("dpRFTruckarmP1Dz"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpTruckarmP1Dz:
                                if (wrapper.containsKey("dpTruckarmP1Dz"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpQtape:
                                if (wrapper.containsKey("dpQtape"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            case DCVariablesEnum.DpFNOMKnobSetting:
                                if (wrapper.containsKey("dpFNOMKnobSetting"))
                                {
                                    dcVars.Add(temp, 0f);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
// ReSharper disable EmptyGeneralCatchClause
                    catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
                    {
                        //if exception is thrown then that var is not for this current car and ignore.
                    }
                }
            }
            return dcVars;
        }
    }
}