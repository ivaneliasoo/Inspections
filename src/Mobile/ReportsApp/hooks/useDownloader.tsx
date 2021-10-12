import RNFetchBlob from 'rn-fetch-blob'
import { useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { API_HOST } from '../config/config';

export const useDownloader =() => {
  const {authState: {userToken}} = useContext(AuthContext)

  const downloadPdf = (id: number) => {
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
    .fetch('GET', `${API_HOST}/api/reports/${id}/export`, {
      Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJkZW1vIiwidW5pcXVlX25hbWUiOiJkZW1vIiwiZnVsbE5hbWUiOiJkZW1vIHVzZXIiLCJzY29wZXMiOiJpbnNwZWN0aW9ucy1yZXBvcnRzIGluc3BlY3Rpb25zLWNvbmZpZ3VyYXRpb24iLCJuYmYiOjE2MzM5NTk0NjQsImV4cCI6MTY2NTA2MzQ2NCwiaWF0IjoxNjMzOTU5NDY0LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxLyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEvIn0._jQP3g6xicy7Ek_pWU6zEfvQ6NvOlEt82MqxFr_rY9o`,
    })
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