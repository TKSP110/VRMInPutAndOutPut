

var ImportVroidAtWebGL = {
  
	ImportVRM: function() {

		if (!document.getElementById('FileImporter')) {

			//JavaScript��input���g�p���܂�
			var fileInput = document.createElement('input');

			//VRM�t�@�C�����w��(�d�l��A�I���t�@�C����VRM�Ɍ��肷�邱�Ƃ͂ł��Ȃ�)
			fileInput.setAttribute('type', 'file');
			fileInput.setAttribute('id', 'FileImporter');
			fileInput.setAttribute('accept', 'application/octet-stream,.vrm')
			fileInput.style.visibility = 'hidden';

			fileInput.onclick = function (event) {
				//�V�[����VRMImporter�I�u�W�F�N�g�ɃA�^�b�`����Ă���R���|�[�l���g��Onclick()�����s����
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