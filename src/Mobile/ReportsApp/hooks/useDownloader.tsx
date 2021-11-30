import RNFetchBlob from 'rn-fetch-blob'
import { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { API_HOST } from '../config/config';

export const useDownloader =() => {
  const {authState: {userToken}} = useContext(AuthContext)
  const android = RNFetchBlob.android

  const downloadPdf = (id: number, printPhotos: boolean = false) => {
    RNFetchBlob
    .config({
        addAndroidDownloads : {
            useDownloadManager : true,
            title: `report_${id}_${Date.now()}.pdf`,
            description : 'Genereating and Downloading the report.',
            mime : 'application/pdf',
            notification : true,
            mediaScannable: true,
            path: `${RNFetchBlob.fs.dirs.DownloadDir}/report_${id}_${Date.now()}.pdf`,
        }
    })
    .fetch('GET', `${API_HOST}/api/reports/${id}/export?printPhotos=${printPhotos}`, {
      Authorization: `Bearer ${userToken}`
    })
    .then((resp) => {
      setTimeout(() => {
        console.log(resp.path())
        android.actionViewIntent(resp.path(), 'application/pdf')
      }, 5000);
    })
    .catch(err => console.log(err))
  }

  return {
    downloadPdf
  }
}