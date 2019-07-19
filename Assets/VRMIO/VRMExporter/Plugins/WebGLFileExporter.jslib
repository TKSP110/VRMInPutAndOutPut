
//���̋@�\���������N���X(���O�͂Ȃ�ł���낵��)
var ExportVroidAtWebGL = {

	WebGLDownloadString: function (filename, text) {
	
		// Unity����n���ꂽstring�̃|�C���^����string�ɕϊ�
		var savefilename = Pointer_stringify(filename);
		var savetext = Pointer_stringify(text);

		//Text���_�E�����[�h���邽�߂̃o�C�i���쐬
		var blob = new Blob([savetext], {type: "text/plain"});

		// IE�����u���E�U���̔���
		if(window.navigator.msSaveBlob)
		{
			// IE�Ȃ�Ǝ��֐����g���܂��B
			window.navigator.msSaveBlob(blob, savefilename);
		} else {
			// ����ȊO��a�^�O�𗘗p���ăC�x���g�𔭉΂����܂�
			var a = document.createElement("a");
			a.href = URL.createObjectURL(blob);
			a.target = '_blank';
			a.download = savefilename;''
			a.click();
		}
	},


	WebGLDownloadBytesFile: function (filename, array, size) {
	
		// �����^�f�[�^(�|�C���^)����1byte���ǂݍ����javascript���Ɏ�����ێ�
		var uint8 = new Uint8Array(size);

		for(var i = 0; i < size; i++)
		uint8[i] = HEAP8[array + i];

		// Unity����n���ꂽstring�̃|�C���^����string�ɕϊ�
		var savefilename = Pointer_stringify(filename);

		//glTF�`��(VRM�̎d�l��glTF�̎d�l���Q��)�Ńo�C�i���f�[�^���쐬
		var blob = new Blob([uint8], {type: "model/gltf-binary"});

		// IE�����u���E�U���̔���
		if(window.navigator.msSaveBlob)
		{
			// IE�Ȃ�Ǝ��֐����g���܂��B
			window.navigator.msSaveBlob(blob, savefilename);
		} else {
			// ����ȊO��a�^�O�𗘗p���ăC�x���g�𔭉΂����܂�
			var a = document.createElement("a");
			a.href = URL.createObjectURL(blob);
			a.target = '_blank';
			a.download = savefilename;''
			a.click();
		}
	}
};

//��Œ�`����Ă�N���X�g���܂���[���܂��Ȃ�(Unity�̌����y�[�W�Q��)
mergeInto(LibraryManager.library, ExportVroidAtWebGL);