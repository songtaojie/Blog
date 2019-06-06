$(function () {
    CkEditor.create(document.querySelector('#editor'), {
        placeholder: "开始编写博客!",
        fontSize: {
            options: [9, 11, 13, 'default', 17, 19, 21]
        },
        ckfinder: {
            // Upload the images to the server using the CKFinder QuickUpload command.
            uploadUrl: '/file/upload?command=QuickUpload&type=Images&responseType=json',
            // Define the CKFinder configuration (if necessary).
            options: {
                resourceType: 'Images'
            }
        }
    })
        .then(editor => {
            window._HxEditor = editor;
            $('div[hidden]').removeAttr('hidden');
            editor.editing.view.document.on('clipboardInput', (evt, data) => {
                const dataTransfer = data.dataTransfer,
                    isFiles = Array.from(dataTransfer.types).includes('Files');
                if (CkEditor.isHtmlIncluded(data.dataTransfer) && !isFiles) {
                    return;
                }
                const images = Array.from(data.dataTransfer.files).filter(file => {
                    if (!file) {
                        return false;
                    }
                    return CkEditor.isImageType(file);
                });
                const ranges = data.targetRanges.map(viewRange => editor.editing.mapper.toModelRange(viewRange));
                editor.model.change(writer => {
                    // Set selection to paste target.
                    writer.setSelection(ranges);
                    if (images.length) {
                        evt.stop();
                        // Upload images after the selection has changed in order to ensure the command's state is refreshed.
                        editor.model.enqueueChange('default', () => {
                            editor.execute('imageUpload', { file: images });
                        });
                    }
                });

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
});