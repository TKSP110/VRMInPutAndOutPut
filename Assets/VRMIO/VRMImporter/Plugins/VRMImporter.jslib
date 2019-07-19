

var ImportVroidAtWebGL = {
  
	ImportVRM: function() {

		if (!document.getElementById('FileImporter')) {

			//JavaScriptのinputを使用します
			var fileInput = document.createElement('input');

			//VRMファイルを指定(仕様上、選択ファイルをVRMに限定することはできない)
			fileInput.setAttribute('type', 'file');
			fileInput.setAttribute('id', 'FileImporter');
			fileInput.setAttribute('accept', 'application/octet-stream,.vrm')
			fileInput.style.visibility = 'hidden';

			fileInput.onclick = function (event) {
				//シーンのVRMImporterオブジェクトにアタッチされているコンポーネントのOnclick()を実行する
				SendMessage('VRMIOImporter', 'ClickCallBackMessageFromPlugin');
				this.value = null;
			};
			
			fileInput.onchange = function (event) {
				SendMessage('VRMIOImporter', 'NameCallBackFromPlugin', event.target.files[0].name);
				var nBytes = 0;
				nBytes += event.target.files[0].size + "byte";
				SendMessage('VRMIOImporter', 'SizeCallBackFromPlugin', nBytes);
				SendMessage('VRMIOImporter', 'URLCallBackMessageFromPlugin', URL.createObjectURL(event.target.files[0]));
				SendMessage('VRMIOImporter', 'SelectedCallBackFromPlugin');
			}
				
			document.body.appendChild(fileInput);
		}

		var OpenFileDialog = function() {

		document.getElementById('FileImporter').click();
		document.getElementById('#canvas').removeEventListener('click', OpenFileDialog);

		};

		document.getElementById('#canvas').addEventListener('click', OpenFileDialog, false);

	}
};

mergeInto(LibraryManager.library, ImportVroidAtWebGL);