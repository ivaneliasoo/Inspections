import { AuthContext } from '../contexts/AuthContext';
import { useContext } from 'react';
import Upload, { UploadOptions } from 'react-native-background-upload';
import { API_HOST, API_KEY } from '../config/config';
import { showMessage } from 'react-native-flash-message';
import { useTheme } from '@ui-kitten/components';
import { Configuration, ReportsApi } from '../services/api';

export const usePhotoRecords = () => {
  const {
    authState: { userToken },
  } = useContext(AuthContext);

  const configuration = new Configuration({
    accessToken: userToken!,
    basePath: API_HOST,
    apiKey: API_KEY,
  });

  const reportsApi = new ReportsApi(configuration);

  const theme = useTheme();
  const uploadOptions: UploadOptions = {
    url: `${API_HOST}/api/Reports/{id}/photorecord`,
    path: '{file}',
    method: 'POST',
    type: 'multipart',
    field: 'uploadedPhoto',
    headers: {
      // 'content-type': 'application/json', // Customize content-type
      'X-API-KEY': API_KEY,
      Authorization: `bearer ${userToken}`,
      label: '',
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
    //useUtf8Charset: true
  };

  const EnqueuePhotoUpload = (
    reportId: number,
    path: string,
    label: string,
  ) => {
    uploadOptions.url = uploadOptions.url.replace('{id}', reportId.toString());
    uploadOptions.path = uploadOptions.path
      .replace('{file}', path)
      .replace('file://', '');
    uploadOptions.headers = { ...uploadOptions.headers, label: label ?? 'NO LABEL' };
    console.log({ uploadOptions });
    Upload.startUpload(uploadOptions)
      .then((uploadId) => {
        Upload.addListener('error', uploadId, (data) => {
          showMessage({
            message: 'An Error has ocurred while trying uploading file',
            autoHide: true,
            backgroundColor: theme['color-error-500'],
          });
        });
        Upload.addListener('cancelled', uploadId, (data) => {});
        Upload.addListener('completed', uploadId, (data) => {
          // data includes responseCode: number and responseBody: Object
          showMessage({
            message: 'PhotoRecord Uploaded Successfully',
            autoHide: true,
            backgroundColor: theme['color-success-500'],
          });
        });
      })
      .catch((err) => {
        console.log('Upload error!', err);
      });
  };

  const GetByReportId = async (id: number): Promise<any[]> => {
    const result = await reportsApi.apiReportsIdPhotorecordGet({ id });
    return result as unknown as any[];
  };

  const deletePhoto = async (reportId: number, id: number): Promise<any[]> => {
    const result = await reportsApi.apiReportsIdPhotorecordIdPhotoDelete({
      id: reportId,
      idPhoto: id,
    });
    return result as unknown as any[];
  };
  const editLabel = async (
    reportId: number,
    id: number,
    label?: string,
  ): Promise<any[]> => {
    const result = await reportsApi.apiReportsIdPhotorecordIdPhotoPut({
      id: reportId,
      idPhoto: id,
      editPhotoRecordCommand: { reportId, id, label: label ?? 'NO LABEL' },
    });
    return result as unknown as any[];
  };

  return {
    EnqueuePhotoUpload,
    GetByReportId,
    deletePhoto,
    editLabel,
  };
};
