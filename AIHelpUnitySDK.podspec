Pod::Spec.new do |s|
  s.name              = "AIHelpUnitySDK"
  s.version           = "2.1.2"
  s.summary           = "AIHelpUnitySDK for iOS"
  s.homepage          = "https://github.com/AI-HELP/unity-example"
  s.license      = { :type => "Apache-2.0", :file => "LICENSE" }
  s.author            = { "AIHelp" => "service_admin@aihelp.net" }
  s.platform          = :ios
  s.platform          = :ios, "9.0"
  s.requires_arc      = true

  s.source            = { :git => "https://github.com/AI-HELP/unity-example.git", :tag => "#{s.version}" }
  s.source_files = 'AIHelpUnitySDK/*.{h,m}'
  s.private_header_files = 'AIHelpUnitySDK/*.h'
  s.dependency 'AIHelpSDK', '~> 2.1.0'
  s.pod_target_xcconfig = { 'VALID_ARCHS' => 'x86_64 armv7 arm64' }

end
