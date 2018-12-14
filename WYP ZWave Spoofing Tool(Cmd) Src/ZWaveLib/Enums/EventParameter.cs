/*
  This file is part of ZWaveLib (https://github.com/genielabs/zwave-lib-dotnet)

  Copyright (2012-2018) G-Labs (https://github.com/genielabs)

  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
*/

/*
 *     Author: Generoso Martello <gene@homegenie.it>
 *     Project Homepage: https://github.com/genielabs/zwave-lib-dotnet
 */

namespace ZWaveLib
{

    public enum EventParameter
    {
        Basic,
        SwitchBinary,
        SwitchMultilevel,
        ManufacturerSpecific,
        MeterKwHour,
        MeterKvaHour,
        MeterWatt,
        MeterPulses,
        MeterAcVolt,
        MeterAcCurrent,
        MeterPower,
        SensorGeneric,
        SensorTemperature,
        SensorHumidity,
        SensorLuminance,
        SensorMotion,
        AlarmGeneric,
        AlarmDoorWindow,
        AlarmSmoke,
        AlarmCarbonMonoxide,
        AlarmCarbonDioxide,
        AlarmHeat,
        AlarmFlood,
        AlarmTampered,
        Configuration,
        WakeUpInterval,
        WakeUpNotify,
        WakeUpSleepingStatus,
        Association,
        VersionCommandClass,
        Battery,
        NodeInfo,
        MultiinstanceSwitchBinaryCount,
        MultiinstanceSwitchBinary,
        MultiinstanceSwitchMultilevelCount,
        MultiinstanceSwitchMultilevel,
        MultiinstanceSensorBinaryCount,
        MultiinstanceSensorBinary,
        MultiinstanceSensorMultilevelCount,
        MultiinstanceSensorMultilevel,
        ThermostatFanMode,
        ThermostatFanState,
        ThermostatHeating,
        ThermostatMode,
        ThermostatOperatingState,
        ThermostatSetBack,
        ThermostatSetPoint,
        UserCode,
        SecurityNodeInformationFrame,
        SecurityDecriptedMessage,
        SecurityGeneratedKey,
        DoorLockStatus,
        RoutingInfo,
        Clock,
        CentralSceneNotification,
        CentralSceneSupportedReport,

        IrrigationSystemInfoReport,
        IrrigationSystemStatusReport,
        IrrigationSystemConfigReport,
        IrrigationValveInfoReport,
        IrrigationValveConfigReport,
        IrrigationValveTableReport,

        WaterFlow,
        WaterPressure,

        ClimateControlSchedule,
        ClimateControlScheduleChanged,
        ClimateControlScheduleOverride,

        Ultraviolet,

        CapabilityReport
    }

}
