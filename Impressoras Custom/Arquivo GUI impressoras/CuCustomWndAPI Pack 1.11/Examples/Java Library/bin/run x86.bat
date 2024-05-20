@echo off

set oldcp=%classpath%

set classpath=CuCustomWndAPIJWrap.jar

java -jar DemoCuCustomWndAPIJWrap.jar

set classpath=%oldcp%