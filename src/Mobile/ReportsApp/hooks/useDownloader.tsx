import RNFetchBlob from 'rn-fetch-blob'
import { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { API_HOST } from '../config/config';

export const useDownloader =() => {
  const {authState: {userToken}} = useContext(AuthContext)

  const downloadPdf = (id: number, printPhotos: boolean = false) => {
    RNFetchBlob
    .config({
        fileCache: true,
        indicator: true,
        overwrite: true,
        addAndroidDownloads : {
            title: 'report.pdf',
            useDownloadManager : true,
            notification : true,
            mime : 'application/pdf',
            description : 'Genereating and Downloading the report.',
            mediaScannable: true,
            path: `${RNFetchBlob.fs.dirs.DownloadDir}/report.pdf`,
        }
    })
    .fetch('GET', `${API_HOST}/api/reports/${id}/export/printPhotos=${printPhotos}`)
    .then((resp) => {
      // the path of downloaded file
      resp.path()
    })
    .catch(err => console.log(err))
  }

  return {
    downloadPdf
  }
}