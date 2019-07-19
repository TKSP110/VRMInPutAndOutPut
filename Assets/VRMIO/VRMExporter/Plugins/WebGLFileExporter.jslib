
//この機能を持ったクラス(名前はなんでもよろしい)
var ExportVroidAtWebGL = {

	WebGLDownloadString: function (filename, text) {
	
		// Unityから渡されたstringのポインタからstringに変換
		var savefilename = Pointer_stringify(filename);
		var savetext = Pointer_stringify(text);

		//Textをダウンロードするためのバイナリ作成
		var blob = new Blob([savetext], {type: "text/plain"});

		// IEか他ブラウザかの判定
		if(window.navigator.msSaveBlob)
		{
			// IEなら独自関数を使います。
			window.navigator.msSaveBlob(blob, savefilename);
		} else {
			// それ以外はaタグを利用してイベントを発火させます
			var a = document.createElement("a");
			a.href = URL.createObjectURL(blob);
			a.target = '_blank';
			a.download = savefilename;''
			a.click();
		}
	},


	WebGLDownloadBytesFile: function (filename, array, size) {
	
		// 複合型データ(ポインタ)から1byteずつ読み込んでjavascript側に実数を保持
		var uint8 = new Uint8Array(size);

		for(var i = 0; i < size; i++)
		uint8[i] = HEAP8[array + i];

		// Unityから渡されたstringのポインタからstringに変換
		var savefilename = Pointer_stringify(filename);

		//glTF形式(VRMの仕様とglTFの仕様を参照)でバイナリデータを作成
		var blob = new Blob([uint8], {type: "model/gltf-binary"});

		// IEか他ブラウザかの判定
		if(window.navigator.msSaveBlob)
		{
			// IEなら独自関数を使います。
			window.navigator.msSaveBlob(blob, savefilename);
		} else {
			// それ以外はaタグを利用してイベントを発火させます
			var a = document.createElement("a");
			a.href = URL.createObjectURL(blob);
			a.target = '_blank';
			a.download = savefilename;''
			a.click();
		}
	}
};

//上で定義されてるクラス使いますよーおまじない(Unityの公式ページ参照)
mergeInto(LibraryManager.library, ExportVroidAtWebGL);