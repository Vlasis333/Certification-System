import axios from "axios";
// import React from "react";
// import { CKEditor } from "@ckeditor/ckeditor5-react";
// import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
// import SimpleUploadAdapter from "@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter";

// const MyEditor = (props) => {
//   const handleEditorReady = (editor) => {
//     console.log("Editor is ready!", editor);
//   };

//   const handleChange = (event, editor) => {
//     console.log("Editor data has changed!", editor.getData());
//   };

//   return (
//     <CKEditor
//       editor={ClassicEditor}
//       config={{
//         plugins: [SimpleUploadAdapter],
//         simpleUpload: {
//           uploadUrl: "https://localhost:7196/api/File",
//         },
//       }}
//       data={props.initialData}
//       onReady={handleEditorReady}
//       onChange={handleChange}
//     />
//   );
// };


// export default MyEditor;

import React from "react";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const API_URL = "https://localhost:7196/api";
const UPLOAD_ENDPOINT = "File";

export default function MyEditor({ handleChange, ...props }) 
{
    function uploadAdapter(loader) {
        return {
            upload: () => {
                return new Promise((resolve, reject) => {
                    const body = new FormData();
                    loader.file.then((file) => {
                        body.append("file", file);
                        // let headers = new Headers();
                        // headers.append("Origin", "http://localhost:3000");
                        axios.post(`${API_URL}/${UPLOAD_ENDPOINT}`, body)
                            // .then((res) => res.json())
                            .then((res) => {
                                resolve({
                                    default: `${res.data.url}`,
                                });
                                console.log(res.url)
                            })
                            .catch((err) => {
                                reject(err);
                            });
                    });
                });
            },
        };
    }
    function uploadPlugin(editor) {
        editor.plugins.get("FileRepository").createUploadAdapter = (loader) => {
            return uploadAdapter(loader);
        };
    }
    return (

        <div className="App">
            <CKEditor
                config={{
                    extraPlugins: [uploadPlugin],
                }}
                editor={ClassicEditor}
                data = {props.text}
                onReady={(editor) => {}}
                onBlur={(event, editor) => {}}
                onFocus={(event, editor) => {}}
                
                onChange={(event, editor) => 
                    {

                  handleChange(event,props.name,editor.getData(),props.index);
                }}
                {...props}

                required
            />
        </div>

    );
}


// fetch(`${API_URL}/${UPLOAD_ENDPOINT}`, {
//     method: "post",
//     body: body,
//     // mode: "no-cors"
// })
//     .then((res) => res.json())
//     .then((res) => {
//         resolve({
//             default: `${res.url}`,
//         });
//         console.log(res.url)
//     })
//     .catch((err) => {
//         reject(err);
//     });
// });
