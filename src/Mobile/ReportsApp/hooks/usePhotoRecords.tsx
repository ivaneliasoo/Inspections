import { AuthContext } from '../contexts/AuthContext';
import { useContext } from 'react';
import Upload, { UploadOptions } from 'react-native-background-upload'
import { API_HOST, API_KEY } from '../config/config';
import { showMessage } from 'react-native-flash-message';
import { useTheme } from '@ui-kitten/components';

export const usePhotoRecords = () => {
  const { authState: { userToken } } = useContext(AuthContext)
  const theme = useTheme()
  const uploadOptions: UploadOptions = {
    url: `${API_HOST}/Reports/{id}/photorecord`,
    path: '{file}',
    method: 'POST',
    type: 'multipart',
    maxRetries: 5, // set retry count (Android only). Default 2
    field: 'uploadedPhoto',
    headers: {
      // 'content-type': 'application/json', // Customize content-type
      'X-API-KEY': API_KEY,
      'Authorization': `bearer ${userToken}`,
      'label': ''
    },
    // Below are options only supported on Android
    notification: {
      enabled: true,
      autoClear: false,
      onProgressTitle: 'Uploading PhotoRecord',
      onCompleteTitle: 'Upload Completed',
      onCompleteMessage: 'PhotoRecord Uploaded Successfully',
      enableRingTone: true,
      onErrorTitle: 'Upload Error',
      onErrorMessage: 'Error uploading PhotoRecord',
    },
    useUtf8Charset: true
  }

  const EnqueuePhotoUpload = (reportId: number, path: string, label: string) => {
    uploadOptions.url = uploadOptions.url.replace('{id}', reportId.toString())
    uploadOptions.path = uploadOptions.path.replace('{file}', path).replace('file://', '')
    uploadOptions.headers = { ...uploadOptions.headers, label }
    console.log({ uploadOptions })
    Upload.startUpload(uploadOptions).then((uploadId) => {
      // Upload.addListener('progress', uploadId, (data) => {
      //   showMessage({
      //     message: 'Photo Upload has been enqueued',
      //     autoHide: true,
      //     backgroundColor: theme['color-info-500']
      //   })
      // })
      Upload.addListener('error', uploadId, (data) => {
        showMessage({
          message: 'An Error has ocurred while trying uploading file',
          autoHide: true,
          backgroundColor: theme['color-error-500']
        })
      })
      Upload.addListener('cancelled', uploadId, (data) => {
      })
      Upload.addListener('completed', uploadId, (data) => {
        // data includes responseCode: number and responseBody: Object
        showMessage({
          message: 'PhotoRecord Uploaded Successfully',
          autoHide: true,
          backgroundColor: theme['color-success-500']
        })
      })
    }).catch((err) => {
      console.log('Upload error!', err)
    })
  }
  return {
    EnqueuePhotoUpload
  }
}
