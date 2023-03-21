# Test 방법

* TestPlugin1, TestPlugin2 Build
* AdaptiveAssemblyLoaderTests.AdaptiveAssemblyLoaderTest 수행

# 사용시 주의사항

* AdaptiveAssemblyLoader 는 꼭 IDisposable Interface의 파생 클래스 내부에서 사용
* 파생 클래스에 Dispose 메서드에서 AdaptiveAssemblyLoader Dispose 수행
* Assembly Unload를 할려면 파생 클래스의 Dispose 호출을 해야 Load 된 Assembly 가 Unload 됨.