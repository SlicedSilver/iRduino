using System.Collections.Generic;
using iRSDKSharp;
using iRacingSdkWrapper.Bitfields;

namespace iRacingSdkWrapper
{
    /// <summary>
    /// Represents an object from which you can get Telemetry var headers by name
    /// </summary>
    public sealed class TelemetryInfo
    {
        private readonly iRacingSDK sdk;

        public TelemetryInfo(iRacingSDK sdk)
        {
            this.sdk = sdk;
        }

        public IEnumerable<TelemetryValue> GetValues()
        {
            var values = new List<TelemetryValue>();
            values.AddRange(new TelemetryValue[]
                                {
                                    this.SessionTime,
                                    this.SessionTimeRemaining,
                                    this.SessionLapsRemaining,
                                    this.SessionNum,
                                    this.SessionState,
                                    this.SessionUniqueID,
                                    this.SessionFlags,
                                    this.DriverMarker,
                                    this.IsReplayPlaying,
                                    this.ReplayFrameNum,
                                    this.CarIdxLap,
                                    this.CarIdxLapDistPct,
                                    this.CarIdxTrackSurface,
                                    this.CarIdxOnPitRoad,
                                    this.CarIdxSteer,
                                    this.CarIdxRPM,
                                    this.CarIdxGear,
                                    this.SteeringWheelAngle,
                                    this.Throttle,
                                    this.Brake,
                                    this.Clutch,
                                    this.Gear,
                                    this.RPM,
                                    this.Lap,
                                    this.LapBestLap,
                                    this.LapLastLapTime,
                                    this.LapBestLapTime,
                                    this.LapCurrentLapTime,
                                    this.LapDeltaToBestLap,
                                    this.LapDeltaToBestLap_DD,
                                    this.LapDeltaToBestLap_OK,
                                    this.LapDeltaToOptimalLap,
                                    this.LapDeltaToOptimalLap_DD,
                                    this.LapDeltaToOptimalLap_OK,
                                    this.LapDeltaToSessionBestLap,
                                    this.LapDeltaToSessionBestLap_DD,
                                    this.LapDeltaToSessionBestLap_OK,
                                    this.LapDeltaToSessionOptimalLap,
                                    this.LapDeltaToSessionOptimalLap_DD,
                                    this.LapDeltaToSessionOptimalLap_OK,
                                    this.LapDist,
                                    this.LapDistPct,
                                    this.RaceLaps,
                                    this.LongAccel,
                                    this.LatAccel,
                                    this.VertAccel,
                                    this.RollRate,
                                    this.PitchRate,
                                    this.YawRate,
                                    this.Speed,
                                    this.VelocityX,
                                    this.VelocityY,
                                    this.VelocityZ,
                                    this.Yaw,
                                    this.Pitch,
                                    this.Roll,
                                    this.CamCarIdx,
                                    this.CamCameraNumber,
                                    this.CamCameraState,
                                    this.CamGroupNumber,
                                    this.IsOnTrack,
                                    this.IsInGarage,
                                    this.SteeringWheelTorque,
                                    this.SteeringWheelPctTorque,
                                    this.ShiftIndicatorPct,
                                    this.EngineWarnings,
                                    this.FuelLevel,
                                    this.FuelLevelPct,
                                    this.ReplayPlaySpeed,
                                    this.ReplaySessionTime,
                                    this.ReplaySessionNum,
                                    this.WaterTemp,
                                    this.WaterLevel,
                                    this.FuelPress,
                                    this.OilTemp,
                                    this.OilPress,
                                    this.OilLevel,
                                    this.Voltage,
                                    this.ManifoldPressure,
                                    this.dcABS,
                                    this.dcBrakeBias,
                                    this.dcAntiRollFront,
                                    this.dcAntiRollRear,
                                    this.dcFuelMixture,
                                    this.dcBoostLevel,
                                    this.dcRevLimiter,
                                    this.dcWeightJackerLeft,
                                    this.dcWeightJackerRight,
                                    this.dcTractionControl,
                                    this.dcTractionControl2,
                                    this.dcWingFront,
                                    this.dcWingRear,
                                    this.dcDiffEntry,
                                    this.dcDiffMiddle,
                                    this.dcDiffExit,
                                    this.dcEngineBraking,
                                    this.dcEnginePower,
                                    this.dcThrottleShape,
                                    this.dpFWingIndex,
                                    this.dpRWingIndex,
                                    this.dpRWingSetting,
                                    this.dpWedgeAdj,
                                    this.dpPSSetting,
                                    this.dpRRDamperPerchOffsetm,
                                    this.dpRBarSetting,
                                    this.dpRFTruckarmP1Dz,
                                    this.dpTruckarmP1Dz,
                                    this.dpQtape,
                                    this.dpFNOMKnobSetting
                                });
            return values;
        }

        /// <summary>
        /// Seconds since session start. Unit: s
        /// </summary>
        public TelemetryValue<double> SessionTime { get { return new TelemetryValue<double>(sdk, "SessionTime"); } }

        /// <summary>
        /// Seconds left in session. Unit: s
        /// </summary>
        public TelemetryValue<double> SessionTimeRemaining { get { return new TelemetryValue<double>(sdk, "SessionTimeRemain"); } }

        /// <summary>
        /// Session Laps remaining. 
        /// </summary>
        public TelemetryValue<int> SessionLapsRemaining { get { return new TelemetryValue<int>(sdk, "SessionLapsRemain"); } }
        
        /// <summary>
        /// Session number. 
        /// </summary>
        public TelemetryValue<int> SessionNum { get { return new TelemetryValue<int>(sdk, "SessionNum"); } }


        /// <summary>
        /// Session state. Unit: irsdk_SessionState
        /// </summary>
        public TelemetryValue<SessionStates> SessionState { get { return new TelemetryValue<SessionStates>(sdk, "SessionState"); } }


        /// <summary>
        /// Session ID. 
        /// </summary>
        public TelemetryValue<int> SessionUniqueID { get { return new TelemetryValue<int>(sdk, "SessionUniqueID"); } }


        /// <summary>
        /// Session flags. Unit: irsdk_Flags
        /// </summary>
        public TelemetryValue<SessionFlag> SessionFlags { get { return new TelemetryValue<SessionFlag>(sdk, "SessionFlags"); } }


        /// <summary>
        /// Driver activated flag. 
        /// </summary>
        public TelemetryValue<bool> DriverMarker { get { return new TelemetryValue<bool>(sdk, "DriverMarker"); } }


        /// <summary>
        /// 0=replay not playing  1=replay playing. 
        /// </summary>
        public TelemetryValue<bool> IsReplayPlaying { get { return new TelemetryValue<bool>(sdk, "IsReplayPlaying"); } }


        /// <summary>
        /// Integer replay frame number (60 per second). 
        /// </summary>
        public TelemetryValue<int> ReplayFrameNum { get { return new TelemetryValue<int>(sdk, "ReplayFrameNum"); } }


        /// <summary>
        /// Lap count by car index. 
        /// </summary>
        public TelemetryValue<int[]> CarIdxLap { get { return new TelemetryValue<int[]>(sdk, "CarIdxLap"); } }


        /// <summary>
        /// Percentage distance around lap by car index. Unit: %
        /// </summary>
        public TelemetryValue<float[]> CarIdxLapDistPct { get { return new TelemetryValue<float[]>(sdk, "CarIdxLapDistPct"); } }


        /// <summary>
        /// Track surface type by car index. Unit: irsdk_TrkLoc
        /// </summary>
        public TelemetryValue<TrackSurfaces[]> CarIdxTrackSurface { get { return new TelemetryValue<TrackSurfaces[]>(sdk, "CarIdxTrackSurface"); } }

        /// <summary>
        /// On Pit Road? by car index. Unit: boolean
        /// </summary>
        public TelemetryValue<bool[]> CarIdxOnPitRoad { get { return new TelemetryValue<bool[]>(sdk, "CarIdxOnPitRoad"); } }

        /// <summary>
        /// Steering wheel angle by car index. Unit: rad
        /// </summary>
        public TelemetryValue<float[]> CarIdxSteer { get { return new TelemetryValue<float[]>(sdk, "CarIdxSteer"); } }


        /// <summary>
        /// Engine rpm by car index. Unit: revs/min
        /// </summary>
        public TelemetryValue<float[]> CarIdxRPM { get { return new TelemetryValue<float[]>(sdk, "CarIdxRPM"); } }


        /// <summary>
        /// -1=reverse  0=neutral  1..n=current gear by car index. 
        /// </summary>
        public TelemetryValue<int[]> CarIdxGear { get { return new TelemetryValue<int[]>(sdk, "CarIdxGear"); } }


        /// <summary>
        /// Steering wheel angle. Unit: rad
        /// </summary>
        public TelemetryValue<float> SteeringWheelAngle { get { return new TelemetryValue<float>(sdk, "SteeringWheelAngle"); } }


        /// <summary>
        /// 0=off throttle to 1=full throttle. Unit: %
        /// </summary>
        public TelemetryValue<float> Throttle { get { return new TelemetryValue<float>(sdk, "Throttle"); } }


        /// <summary>
        /// 0=brake released to 1=max pedal force. Unit: %
        /// </summary>
        public TelemetryValue<float> Brake { get { return new TelemetryValue<float>(sdk, "Brake"); } }


        /// <summary>
        /// 0=disengaged to 1=fully engaged. Unit: %
        /// </summary>
        public TelemetryValue<float> Clutch { get { return new TelemetryValue<float>(sdk, "Clutch"); } }


        /// <summary>
        /// -1=reverse  0=neutral  1..n=current gear. 
        /// </summary>
        public TelemetryValue<int> Gear { get { return new TelemetryValue<int>(sdk, "Gear"); } }


        /// <summary>
        /// Engine rpm. Unit: revs/min
        /// </summary>
        public TelemetryValue<float> RPM { get { return new TelemetryValue<float>(sdk, "RPM"); } }


        /// <summary>
        /// Lap count. 
        /// </summary>
        public TelemetryValue<int> Lap { get { return new TelemetryValue<int>(sdk, "Lap"); } }

        /// <summary>
        /// Best Lap Number 
        /// </summary>
        public TelemetryValue<int> LapBestLap { get { return new TelemetryValue<int>(sdk, "LapBestLap"); } }

        /// <summary>
        /// Last Lap Time 
        /// </summary>
        public TelemetryValue<float> LapLastLapTime { get { return new TelemetryValue<float>(sdk, "LapLastLapTime"); } }


        /// <summary>
        /// Best Lap Time 
        /// </summary>
        public TelemetryValue<float> LapBestLapTime { get { return new TelemetryValue<float>(sdk, "LapBestLapTime"); } }

        /// <summary>
        /// Current Lap Time.
        /// </summary>
        public TelemetryValue<float> LapCurrentLapTime { get { return new TelemetryValue<float>(sdk, "LapCurrentLapTime"); } }


        /// <summary>
        /// Lap Delta Time To Best Lap
        /// </summary>
        public TelemetryValue<float> LapDeltaToBestLap { get { return new TelemetryValue<float>(sdk, "LapDeltaToBestLap"); } }

        /// <summary>
        /// Lap Delta Time To Best Lap. DD Colour Percentage
        /// </summary>
        public TelemetryValue<float> LapDeltaToBestLap_DD { get { return new TelemetryValue<float>(sdk, "LapDeltaToBestLap_DD"); } }

        /// <summary>
        /// Lap Delta Time To Best Lap is Valid Boolean
        /// </summary>
        public TelemetryValue<bool> LapDeltaToBestLap_OK { get { return new TelemetryValue<bool>(sdk, "LapDeltaToBestLap_OK"); } }

        /// <summary>
        /// Lap Delta Time To Optimal Lap
        /// </summary>
        public TelemetryValue<float> LapDeltaToOptimalLap { get { return new TelemetryValue<float>(sdk, "LapDeltaToOptimalLap"); } }

        /// <summary>
        /// Lap Delta Time To Optimal Lap. DD Colour Percentage
        /// </summary>
        public TelemetryValue<float> LapDeltaToOptimalLap_DD { get { return new TelemetryValue<float>(sdk, "LapDeltaToOptimalLap_DD"); } }

        /// <summary>
        /// Lap Delta Time To Optimal Lap is Valid Boolean
        /// </summary>
        public TelemetryValue<bool> LapDeltaToOptimalLap_OK { get { return new TelemetryValue<bool>(sdk, "LapDeltaToOptimalLap_OK"); } }

        /// <summary>
        /// Lap Delta Time To Session Best Lap
        /// </summary>
        public TelemetryValue<float> LapDeltaToSessionBestLap { get { return new TelemetryValue<float>(sdk, "LapDeltaToSessionBestLap"); } }

        /// <summary>
        /// Lap Delta Time To Session Best Lap. DD Colour Percentage
        /// </summary>
        public TelemetryValue<float> LapDeltaToSessionBestLap_DD { get { return new TelemetryValue<float>(sdk, "LapDeltaToSessionBestLap_DD"); } }

        /// <summary>
        /// Lap Delta Time To Session Best Lap is Valid Boolean
        /// </summary>
        public TelemetryValue<bool> LapDeltaToSessionBestLap_OK { get { return new TelemetryValue<bool>(sdk, "LapDeltaToSessionBestLap_OK"); } }

        /// <summary>
        /// Lap Delta Time To Session Optimal Lap
        /// </summary>
        public TelemetryValue<float> LapDeltaToSessionOptimalLap { get { return new TelemetryValue<float>(sdk, "LapDeltaToSessionOptimalLap"); } }

        /// <summary>
        /// Lap Delta Time To Session Optimal Lap. DD Colour Percentage
        /// </summary>
        public TelemetryValue<float> LapDeltaToSessionOptimalLap_DD { get { return new TelemetryValue<float>(sdk, "LapDeltaToSessionOptimalLap_DD"); } }

        /// <summary>
        /// Lap Delta Time To Session Optimal Lap is Valid Boolean
        /// </summary>
        public TelemetryValue<bool> LapDeltaToSessionOptimalLap_OK { get { return new TelemetryValue<bool>(sdk, "LapDeltaToSessionOptimalLap_OK"); } }


        /// <summary>
        /// Meters traveled from S/F this lap. Unit: m
        /// </summary>
        public TelemetryValue<float> LapDist { get { return new TelemetryValue<float>(sdk, "LapDist"); } }


        /// <summary>
        /// Percentage distance around lap. Unit: %
        /// </summary>
        public TelemetryValue<float> LapDistPct { get { return new TelemetryValue<float>(sdk, "LapDistPct"); } }


        /// <summary>
        /// Laps completed in race. 
        /// </summary>
        public TelemetryValue<int> RaceLaps { get { return new TelemetryValue<int>(sdk, "RaceLaps"); } }


        /// <summary>
        /// Longitudinal acceleration (including gravity). Unit: m/s^2
        /// </summary>
        public TelemetryValue<float> LongAccel { get { return new TelemetryValue<float>(sdk, "LongAccel"); } }


        /// <summary>
        /// Lateral acceleration (including gravity). Unit: m/s^2
        /// </summary>
        public TelemetryValue<float> LatAccel { get { return new TelemetryValue<float>(sdk, "LatAccel"); } }


        /// <summary>
        /// Vertical acceleration (including gravity). Unit: m/s^2
        /// </summary>
        public TelemetryValue<float> VertAccel { get { return new TelemetryValue<float>(sdk, "VertAccel"); } }


        /// <summary>
        /// Roll rate. Unit: rad/s
        /// </summary>
        public TelemetryValue<float> RollRate { get { return new TelemetryValue<float>(sdk, "RollRate"); } }


        /// <summary>
        /// Pitch rate. Unit: rad/s
        /// </summary>
        public TelemetryValue<float> PitchRate { get { return new TelemetryValue<float>(sdk, "PitchRate"); } }


        /// <summary>
        /// Yaw rate. Unit: rad/s
        /// </summary>
        public TelemetryValue<float> YawRate { get { return new TelemetryValue<float>(sdk, "YawRate"); } }


        /// <summary>
        /// GPS vehicle speed. Unit: m/s
        /// </summary>
        public TelemetryValue<float> Speed { get { return new TelemetryValue<float>(sdk, "Speed"); } }


        /// <summary>
        /// X velocity. Unit: m/s
        /// </summary>
        public TelemetryValue<float> VelocityX { get { return new TelemetryValue<float>(sdk, "VelocityX"); } }


        /// <summary>
        /// Y velocity. Unit: m/s
        /// </summary>
        public TelemetryValue<float> VelocityY { get { return new TelemetryValue<float>(sdk, "VelocityY"); } }


        /// <summary>
        /// Z velocity. Unit: m/s
        /// </summary>
        public TelemetryValue<float> VelocityZ { get { return new TelemetryValue<float>(sdk, "VelocityZ"); } }


        /// <summary>
        /// Yaw orientation. Unit: rad
        /// </summary>
        public TelemetryValue<float> Yaw { get { return new TelemetryValue<float>(sdk, "Yaw"); } }


        /// <summary>
        /// Pitch orientation. Unit: rad
        /// </summary>
        public TelemetryValue<float> Pitch { get { return new TelemetryValue<float>(sdk, "Pitch"); } }


        /// <summary>
        /// Roll orientation. Unit: rad
        /// </summary>
        public TelemetryValue<float> Roll { get { return new TelemetryValue<float>(sdk, "Roll"); } }


        /// <summary>
        /// Active camera's focus car index. 
        /// </summary>
        public TelemetryValue<int> CamCarIdx { get { return new TelemetryValue<int>(sdk, "CamCarIdx"); } }


        /// <summary>
        /// Active camera number. 
        /// </summary>
        public TelemetryValue<int> CamCameraNumber { get { return new TelemetryValue<int>(sdk, "CamCameraNumber"); } }


        /// <summary>
        /// Active camera group number. 
        /// </summary>
        public TelemetryValue<int> CamGroupNumber { get { return new TelemetryValue<int>(sdk, "CamGroupNumber"); } }


        /// <summary>
        /// State of camera system. Unit: irsdk_CameraState
        /// </summary>
        public TelemetryValue<CameraState> CamCameraState { get { return new TelemetryValue<CameraState>(sdk, "CamCameraState"); } }


        /// <summary>
        /// 1=Car on track physics running. 
        /// </summary>
        public TelemetryValue<bool> IsOnTrack { get { return new TelemetryValue<bool>(sdk, "IsOnTrack"); } }


        /// <summary>
        /// 1=Car in garage physics running. 
        /// </summary>
        public TelemetryValue<bool> IsInGarage { get { return new TelemetryValue<bool>(sdk, "IsInGarage"); } }


        /// <summary>
        /// Output torque on steering shaft. Unit: N*m
        /// </summary>
        public TelemetryValue<float> SteeringWheelTorque { get { return new TelemetryValue<float>(sdk, "SteeringWheelTorque"); } }


        /// <summary>
        /// Force feedback % max torque on steering shaft. Unit: %
        /// </summary>
        public TelemetryValue<float> SteeringWheelPctTorque { get { return new TelemetryValue<float>(sdk, "SteeringWheelPctTorque"); } }


        /// <summary>
        /// Percent of shift indicator to light up. Unit: %
        /// </summary>
        public TelemetryValue<float> ShiftIndicatorPct { get { return new TelemetryValue<float>(sdk, "ShiftIndicatorPct"); } }


        /// <summary>
        /// Bitfield for warning lights. Unit: irsdk_EngineWarnings
        /// </summary>
        public TelemetryValue<EngineWarning> EngineWarnings { get { return new TelemetryValue<EngineWarning>(sdk, "EngineWarnings"); } }


        /// <summary>
        /// Liters of fuel remaining. Unit: l
        /// </summary>
        public TelemetryValue<float> FuelLevel { get { return new TelemetryValue<float>(sdk, "FuelLevel"); } }


        /// <summary>
        /// Percent fuel remaining. Unit: %
        /// </summary>
        public TelemetryValue<float> FuelLevelPct { get { return new TelemetryValue<float>(sdk, "FuelLevelPct"); } }


        /// <summary>
        /// Replay playback speed. 
        /// </summary>
        public TelemetryValue<int> ReplayPlaySpeed { get { return new TelemetryValue<int>(sdk, "ReplayPlaySpeed"); } }


        /// <summary>
        /// 0=not slow motion  1=replay is in slow motion. 
        /// </summary>
        public TelemetryValue<bool> ReplayPlaySlowMotion { get { return new TelemetryValue<bool>(sdk, "ReplayPlaySlowMotion"); } }


        /// <summary>
        /// Seconds since replay session start. Unit: s
        /// </summary>
        public TelemetryValue<double> ReplaySessionTime { get { return new TelemetryValue<double>(sdk, "ReplaySessionTime"); } }


        /// <summary>
        /// Replay session number. 
        /// </summary>
        public TelemetryValue<int> ReplaySessionNum { get { return new TelemetryValue<int>(sdk, "ReplaySessionNum"); } }


        /// <summary>
        /// Engine coolant temp. Unit: C
        /// </summary>
        public TelemetryValue<float> WaterTemp { get { return new TelemetryValue<float>(sdk, "WaterTemp"); } }


        /// <summary>
        /// Engine coolant level. Unit: l
        /// </summary>
        public TelemetryValue<float> WaterLevel { get { return new TelemetryValue<float>(sdk, "WaterLevel"); } }


        /// <summary>
        /// Engine fuel pressure. Unit: bar
        /// </summary>
        public TelemetryValue<float> FuelPress { get { return new TelemetryValue<float>(sdk, "FuelPress"); } }

        /// <summary>
        /// Engine fuel pressure. Unit: bar
        /// </summary>
        public TelemetryValue<float> ManifoldPressure { get { return new TelemetryValue<float>(sdk, "ManifoldPress"); } }

        /// <summary>
        /// Engine oil temperature. Unit: C
        /// </summary>
        public TelemetryValue<float> OilTemp { get { return new TelemetryValue<float>(sdk, "OilTemp"); } }


        /// <summary>
        /// Engine oil pressure. Unit: bar
        /// </summary>
        public TelemetryValue<float> OilPress { get { return new TelemetryValue<float>(sdk, "OilPress"); } }


        /// <summary>
        /// Engine oil level. Unit: l
        /// </summary>
        public TelemetryValue<float> OilLevel { get { return new TelemetryValue<float>(sdk, "OilLevel"); } }


        /// <summary>
        /// Engine voltage. Unit: V
        /// </summary>
        public TelemetryValue<float> Voltage { get { return new TelemetryValue<float>(sdk, "Voltage"); } }

        /// <summary>
        /// Dashboard Control: ABS
        /// </summary>
// ReSharper disable InconsistentNaming
        public TelemetryValue<float> dcABS { get { return new TelemetryValue<float>(sdk, "dcABS"); } }

        public TelemetryValue<float> dcBrakeBias { get { return new TelemetryValue<float>(sdk, "dcBrakeBias"); } }
        public TelemetryValue<float> dcAntiRollFront { get { return new TelemetryValue<float>(sdk, "dcAntiRollFront"); } }
        public TelemetryValue<float> dcAntiRollRear { get { return new TelemetryValue<float>(sdk, "dcAntiRollRear"); } }
        public TelemetryValue<float> dcFuelMixture { get { return new TelemetryValue<float>(sdk, "dcFuelMixture"); } }
        public TelemetryValue<float> dcBoostLevel { get { return new TelemetryValue<float>(sdk, "dcBoostLevel"); } }
        public TelemetryValue<float> dcRevLimiter { get { return new TelemetryValue<float>(sdk, "dcRevLimiter"); } }
        public TelemetryValue<float> dcWeightJackerLeft { get { return new TelemetryValue<float>(sdk, "dcWeightJackerLeft"); } }
        public TelemetryValue<float> dcWeightJackerRight { get { return new TelemetryValue<float>(sdk, "dcWeightJackerRight"); } }
        public TelemetryValue<float> dcTractionControl { get { return new TelemetryValue<float>(sdk, "dcTractionControl"); } }
        public TelemetryValue<float> dcTractionControl2 { get { return new TelemetryValue<float>(sdk, "dcTractionControl2"); } }
        public TelemetryValue<float> dcWingFront { get { return new TelemetryValue<float>(sdk, "dcWingFront"); } }
        public TelemetryValue<float> dcWingRear { get { return new TelemetryValue<float>(sdk, "dcWingRear"); } }
        public TelemetryValue<float> dcDiffEntry { get { return new TelemetryValue<float>(sdk, "dcDiffEntry"); } }
        public TelemetryValue<float> dcDiffMiddle { get { return new TelemetryValue<float>(sdk, "dcDiffMiddle"); } }
        public TelemetryValue<float> dcDiffExit { get { return new TelemetryValue<float>(sdk, "dcDiffExit"); } }
        public TelemetryValue<float> dcEngineBraking { get { return new TelemetryValue<float>(sdk, "dcEngineBraking"); } }
        public TelemetryValue<float> dcEnginePower { get { return new TelemetryValue<float>(sdk, "dcEnginePower"); } }
        public TelemetryValue<float> dcThrottleShape { get { return new TelemetryValue<float>(sdk, "dcThrottleShape"); } }
        public TelemetryValue<float> dpFWingIndex { get { return new TelemetryValue<float>(sdk, "dpFWingIndex"); } }
        public TelemetryValue<float> dpRWingIndex { get { return new TelemetryValue<float>(sdk, "dpRWingIndex"); } }
        public TelemetryValue<float> dpRWingSetting { get { return new TelemetryValue<float>(sdk, "dpRWingSetting"); } }
        public TelemetryValue<float> dpWedgeAdj { get { return new TelemetryValue<float>(sdk, "dpWedgeAdj"); } }
        public TelemetryValue<float> dpPSSetting { get { return new TelemetryValue<float>(sdk, "dpPSSetting"); } }
        public TelemetryValue<float> dpRRDamperPerchOffsetm { get { return new TelemetryValue<float>(sdk, "dpRRDamperPerchOffsetm"); } }
        public TelemetryValue<float> dpRBarSetting { get { return new TelemetryValue<float>(sdk, "dpRBarSetting"); } }
        public TelemetryValue<float> dpRFTruckarmP1Dz { get { return new TelemetryValue<float>(sdk, "dpRFTruckarmP1Dz"); } }
        public TelemetryValue<float> dpTruckarmP1Dz { get { return new TelemetryValue<float>(sdk, "dpTruckarmP1Dz"); } }
        public TelemetryValue<float> dpQtape { get { return new TelemetryValue<float>(sdk, "dpQtape"); } }
        public TelemetryValue<float> dpFNOMKnobSetting { get { return new TelemetryValue<float>(sdk, "dpFNOMKnobSetting"); } }
// ReSharper restore InconsistentNaming
    }
}